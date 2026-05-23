using System.Collections.Generic;

namespace CMMS.BLL.Helpers
{
    public static class SensorRangeValidator
    {
        public const double TemperatureMin = -50.0;
        public const double TemperatureMax = 80.0;
        public const double HumidityMin = 0.0;
        public const double HumidityMax = 100.0;
        public const double SoilMoistureMin = 0.0;
        public const double SoilMoistureMax = 100.0;
        public const double LightMin = 0.0;
        public const double LightMax = 200000.0;

        public static List<string> Validate(double? temperature, double? humidity, double? soilMoisture, double? light)
        {
            var errors = new List<string>();

            if (temperature.HasValue && (temperature.Value < TemperatureMin || temperature.Value > TemperatureMax))
                errors.Add($"Nhiệt độ phải từ {TemperatureMin} đến {TemperatureMax} °C (giá trị nhận: {temperature.Value}).");

            if (humidity.HasValue && (humidity.Value < HumidityMin || humidity.Value > HumidityMax))
                errors.Add($"Độ ẩm phải từ {HumidityMin} đến {HumidityMax} % (giá trị nhận: {humidity.Value}).");

            if (soilMoisture.HasValue && (soilMoisture.Value < SoilMoistureMin || soilMoisture.Value > SoilMoistureMax))
                errors.Add($"Độ ẩm đất phải từ {SoilMoistureMin} đến {SoilMoistureMax} % (giá trị nhận: {soilMoisture.Value}).");

            if (light.HasValue && (light.Value < LightMin || light.Value > LightMax))
                errors.Add($"Ánh sáng phải từ {LightMin} đến {LightMax} lux (giá trị nhận: {light.Value}).");

            return errors;
        }

        public static bool IsValid(double? temperature, double? humidity, double? soilMoisture, double? light)
            => Validate(temperature, humidity, soilMoisture, light).Count == 0;
    }
}
