using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using WeatherCollector.Interfaces;

namespace WeatherCollector.Repositories.Entities
{
    public class WeatherEntity : TableEntity
    {
        public DateTime Time { get; set; }
        public double Temperature { get; set; }
        public double RainNext1hrs { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double CloudFraction { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public static List<WeatherEntity> FromWeather(ILogger logger, IWeather weather, string partitonKey, bool earlyUpdate) {
            logger.LogInformation("FromWeather start");
            var weatherList = new List<WeatherEntity>();
            var timeSeries = weather.Properties.Timeseries;
            TimeSpan start = earlyUpdate ? new TimeSpan(9, 0, 0) : new TimeSpan(21, 0, 0); // 09:00:00Z or 21:00:00Z
            TimeSpan end = earlyUpdate ? new TimeSpan(20, 59, 59) : new TimeSpan(08, 59, 59); // 20:59:59Z or 08:59:59Z
            for (var i = 0; i < timeSeries.Count; i++)
            {
                var currentWeather = timeSeries[i];
                var now = currentWeather.Time.TimeOfDay;
                logger.LogInformation("currentWeather" + currentWeather);
                logger.LogInformation("now" + now);
                if (InsideTimeSpan(start, end, now))
                {
                    logger.LogInformation("Inside time span");
                    var instantWeatherDetails = currentWeather.Data.Instant.Details;
                    var next1HoursWeatherDetails = currentWeather.Data.Next1Hours.Details;
                    logger.LogInformation("instantWeatherDetails" + instantWeatherDetails);
                    logger.LogInformation("next1HoursWeatherDetails" + next1HoursWeatherDetails);
                    weatherList.Add(new WeatherEntity()
                    {
                        PartitionKey = partitonKey,
                        RowKey = Guid.NewGuid().ToString(),

                        Time = currentWeather.Time,
                        Temperature = instantWeatherDetails.AirTemperature,
                        RainNext1hrs = next1HoursWeatherDetails.PrecipitationAmount,
                        WindSpeed = instantWeatherDetails.WindSpeed,
                        WindDirection = instantWeatherDetails.WindFromDirection,
                        CloudFraction = instantWeatherDetails.CloudAreaFraction,
                        Humidity = instantWeatherDetails.RelativeHumidity,
                        Pressure = instantWeatherDetails.AirPressureAtSeaLevel,

                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                    });
                }
                logger.LogInformation("Not inside");
            }
            logger.LogInformation("FromWeather end");
            return weatherList;
        }

        private static bool InsideTimeSpan(TimeSpan start, TimeSpan end, TimeSpan now)
        {
            if (start <= end)
            {
                // start and stop times are in the same day
                if (now >= start && now <= end)
                {
                    // current time is between start and stop
                    return true;
                }
            }
            else
            {
                // start and stop times are in different days
                if (now >= start || now <= end)
                {
                    // current time is between start and stop
                    return true;
                }
            }
            return false;
        }
    }
}
