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
            CropGrowthStage stage,
            SensorDataRequest reading,
            Guid? sensorDataId,
            DateTime recordedAt)
        {
            if (!_settings.Enabled) return;

            var alerts = BuildAlerts(reading, stage);
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
                        NoteStatus = "unread",
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
                        stageId = stage.StageId,
                        stageName = stage.StageName,
                        alertType = alert.Type,
                        severity = alert.Severity,
                        metric = alert.Metric,
                        value = alert.Value,
                        threshold = alert.Threshold
                    }
                });
            }
        }

        private static List<SensorAlert> BuildAlerts(SensorDataRequest r, CropGrowthStage stage)
        {
            var list = new List<SensorAlert>();

            if (stage.TemperatureMax.HasValue
                && r.Temperature.HasValue
                && r.Temperature.Value > stage.TemperatureMax.Value)
            {
                list.Add(new SensorAlert
                {
                    Type = "high_temperature",
                    Title = "Cảnh báo nhiệt độ vượt ngưỡng",
                    Message = $"Nhiệt độ {r.Temperature.Value:F1}°C vượt ngưỡng {stage.TemperatureMax.Value:F0}°C của giai đoạn '{stage.StageName}'.",
                    Severity = "high",
                    Metric = "temperature",
                    Value = r.Temperature.Value,
                    Threshold = stage.TemperatureMax.Value
                });
            }

            if (stage.HumidityMax.HasValue
                && r.Humidity.HasValue
                && r.Humidity.Value > stage.HumidityMax.Value)
            {
                list.Add(new SensorAlert
                {
                    Type = "high_humidity",
                    Title = "Cảnh báo độ ẩm không khí vượt ngưỡng",
                    Message = $"Độ ẩm không khí {r.Humidity.Value:F1}% vượt ngưỡng {stage.HumidityMax.Value:F0}% của giai đoạn '{stage.StageName}'.",
                    Severity = "warning",
                    Metric = "humidity",
                    Value = r.Humidity.Value,
                    Threshold = stage.HumidityMax.Value
                });
            }

            if (stage.SoilMoistureMax.HasValue
                && r.SoilMoisture.HasValue
                && r.SoilMoisture.Value > stage.SoilMoistureMax.Value)
            {
                list.Add(new SensorAlert
                {
                    Type = "high_soil_moisture",
                    Title = "Cảnh báo độ ẩm đất vượt ngưỡng",
                    Message = $"Độ ẩm đất {r.SoilMoisture.Value:F1}% vượt ngưỡng {stage.SoilMoistureMax.Value:F0}% của giai đoạn '{stage.StageName}'.",
                    Severity = "high",
                    Metric = "soilMoisture",
                    Value = r.SoilMoisture.Value,
                    Threshold = stage.SoilMoistureMax.Value
                });
            }

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
