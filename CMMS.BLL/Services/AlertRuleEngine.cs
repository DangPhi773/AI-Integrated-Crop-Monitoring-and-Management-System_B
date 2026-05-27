using System.Collections.Concurrent;
using CMMS.BLL.Configuration;
using CMMS.BLL.Helpers;
using CMMS.BLL.Interfaces;
using CMMS.BLL.Realtime;
using CMMS.DAL.DTOs.IotDatas;
using CMMS.DAL.Entities;
using CMMS.DAL.Interfaces;
using Microsoft.Extensions.Options;

namespace CMMS.BLL.Services
{
    public class AlertRuleEngine : IAlertRuleEngine
    {
        private static readonly ConcurrentDictionary<string, DateTime> _lastSentUtc = new();

        private readonly SensorAlertSettings _settings;
        private readonly IUserRepository _userRepo;
        private readonly INotificationRepository _notificationRepo;
        private readonly INotificationRealtime _realtime;

        public AlertRuleEngine(
            IOptions<SensorAlertSettings> options,
            IUserRepository userRepo,
            INotificationRepository notificationRepo,
            INotificationRealtime realtime)
        {
            _settings = options.Value;
            _userRepo = userRepo;
            _notificationRepo = notificationRepo;
            _realtime = realtime;
        }

        public async System.Threading.Tasks.Task EvaluateAndNotifyAsync(
            IotDevice device,
            SensorDataRequest reading,
            Guid? sensorDataId,
            DateTime recordedAt)
        {
            if (!_settings.Enabled) return;

            var alerts = BuildAlerts(reading);
            if (alerts.Count == 0) return;

            var nowUtc = DateTime.UtcNow;
            var cooldown = TimeSpan.FromMinutes(Math.Max(1, _settings.CooldownMinutes));
            var deviceKey = device.DeviceId.ToString();

            var dueAlerts = new List<SensorAlert>();
            foreach (var alert in alerts)
            {
                var key = $"{deviceKey}:{alert.Type}";
                if (_lastSentUtc.TryGetValue(key, out var last) && nowUtc - last < cooldown)
                    continue;
                _lastSentUtc[key] = nowUtc;
                dueAlerts.Add(alert);
            }

            if (dueAlerts.Count == 0) return;

            var recipients = await _userRepo.GetByRoleNamesAsync("Owner", "Worker");
            if (recipients.Count == 0) return;

            var bedId = device.BedId;
            var plotId = device.Bed?.PlotId;
            var farmId = device.Bed?.Plot?.FarmId;
            var deviceLabel = !string.IsNullOrWhiteSpace(device.Name)
                ? device.Name
                : (device.DeviceCode ?? device.DeviceId.ToString());

            var createdAt = DateTimeHelper.VnNow();
            var pending = new List<(Notification note, SensorAlert alert)>();

            foreach (var alert in dueAlerts)
            {
                foreach (var user in recipients)
                {
                    var note = new Notification
                    {
                        NoteId = Guid.NewGuid(),
                        UserId = user.UserId,
                        NoteType = "sensor_alert",
                        NoteTitle = alert.Title,
                        NoteMessage = $"{deviceLabel}: {alert.Message}",
                        NoteStatus = "sent",
                        NoteCreatedAt = createdAt
                    };
                    pending.Add((note, alert));
                }
            }

            await _notificationRepo.AddRangeAsync(pending.Select(p => p.note));
            await _notificationRepo.SaveChangesAsync();

            foreach (var (note, alert) in pending)
            {
                await _realtime.PushToUserAsync(note.UserId!.Value, new
                {
                    noteId = note.NoteId,
                    noteType = note.NoteType,
                    noteTitle = note.NoteTitle,
                    noteMessage = note.NoteMessage,
                    createdAt = note.NoteCreatedAt,
                    sensor = new
                    {
                        deviceId = device.DeviceId,
                        deviceCode = device.DeviceCode,
                        bedId,
                        plotId,
                        farmId,
                        sensorDataId,
                        recordedAt,
                        alertType = alert.Type,
                        severity = alert.Severity,
                        metric = alert.Metric,
                        value = alert.Value,
                        threshold = alert.Threshold
                    }
                });
            }
        }

        private List<SensorAlert> BuildAlerts(SensorDataRequest r)
        {
            var list = new List<SensorAlert>();

            if (r.Humidity.HasValue && r.Humidity.Value < _settings.LowHumidityPct)
                list.Add(new SensorAlert
                {
                    Type = "low_humidity",
                    Title = "Cảnh báo độ ẩm không khí thấp",
                    Message = $"Độ ẩm không khí {r.Humidity.Value:F1}% (ngưỡng {_settings.LowHumidityPct:F0}%) — cần tăng cường giữ ẩm cho cây.",
                    Severity = "warning",
                    Metric = "humidity",
                    Value = r.Humidity.Value,
                    Threshold = _settings.LowHumidityPct
                });

            if (r.Humidity.HasValue && r.Humidity.Value > _settings.HighHumidityPct)
                list.Add(new SensorAlert
                {
                    Type = "high_humidity",
                    Title = "Cảnh báo độ ẩm không khí cao",
                    Message = $"Độ ẩm không khí {r.Humidity.Value:F1}% (ngưỡng {_settings.HighHumidityPct:F0}%) — nguy cơ phát sinh nấm bệnh.",
                    Severity = "warning",
                    Metric = "humidity",
                    Value = r.Humidity.Value,
                    Threshold = _settings.HighHumidityPct
                });

            if (r.Temperature.HasValue && r.Temperature.Value > _settings.HighTemperatureC)
                list.Add(new SensorAlert
                {
                    Type = "high_temperature",
                    Title = "Cảnh báo nhiệt độ cao",
                    Message = $"Nhiệt độ {r.Temperature.Value:F1}°C (ngưỡng {_settings.HighTemperatureC:F0}°C) — cần tưới mát hoặc che chắn.",
                    Severity = "high",
                    Metric = "temperature",
                    Value = r.Temperature.Value,
                    Threshold = _settings.HighTemperatureC
                });

            if (r.Temperature.HasValue && r.Temperature.Value < _settings.LowTemperatureC)
                list.Add(new SensorAlert
                {
                    Type = "low_temperature",
                    Title = "Cảnh báo nhiệt độ thấp",
                    Message = $"Nhiệt độ {r.Temperature.Value:F1}°C (ngưỡng {_settings.LowTemperatureC:F0}°C) — nguy cơ rét hại cây trồng.",
                    Severity = "warning",
                    Metric = "temperature",
                    Value = r.Temperature.Value,
                    Threshold = _settings.LowTemperatureC
                });

            if (r.SoilMoisture.HasValue && r.SoilMoisture.Value < _settings.LowSoilMoisturePct)
                list.Add(new SensorAlert
                {
                    Type = "low_soil_moisture",
                    Title = "Cảnh báo độ ẩm đất thấp",
                    Message = $"Độ ẩm đất {r.SoilMoisture.Value:F1}% (ngưỡng {_settings.LowSoilMoisturePct:F0}%) — cần tưới nước ngay.",
                    Severity = "high",
                    Metric = "soilMoisture",
                    Value = r.SoilMoisture.Value,
                    Threshold = _settings.LowSoilMoisturePct
                });

            if (r.SoilMoisture.HasValue && r.SoilMoisture.Value > _settings.HighSoilMoisturePct)
                list.Add(new SensorAlert
                {
                    Type = "high_soil_moisture",
                    Title = "Cảnh báo đất quá ẩm",
                    Message = $"Độ ẩm đất {r.SoilMoisture.Value:F1}% (ngưỡng {_settings.HighSoilMoisturePct:F0}%) — nguy cơ úng rễ, ngừng tưới.",
                    Severity = "warning",
                    Metric = "soilMoisture",
                    Value = r.SoilMoisture.Value,
                    Threshold = _settings.HighSoilMoisturePct
                });

            if (r.Light.HasValue && r.Light.Value > _settings.HighLightLux)
                list.Add(new SensorAlert
                {
                    Type = "high_light",
                    Title = "Cảnh báo cường độ ánh sáng cao",
                    Message = $"Cường độ ánh sáng {r.Light.Value:F0} lux (ngưỡng {_settings.HighLightLux:F0} lux) — cần che chắn cho cây.",
                    Severity = "warning",
                    Metric = "light",
                    Value = r.Light.Value,
                    Threshold = _settings.HighLightLux
                });

            return list;
        }

        private sealed class SensorAlert
        {
            public string Type { get; init; } = string.Empty;
            public string Title { get; init; } = string.Empty;
            public string Message { get; init; } = string.Empty;
            public string Severity { get; init; } = "warning";
            public string Metric { get; init; } = string.Empty;
            public double Value { get; init; }
            public double Threshold { get; init; }
        }
    }
}
