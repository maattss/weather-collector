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

        public static List<WeatherEntity> FromWeather(ILogger logger, IWeather weather, string partitonKey) {
            var weatherList = new List<WeatherEntity>();
            var timeSeries = weather.Properties.Timeseries;

            bool nightUpdate = false;
            bool dayUpdate = true; // TODO: Should be set to false!
  
            /*if (DateTime.Now.Hour == 0)
            {
                nightUpdate = true;
            } 
            else if (DateTime.Now.Hour == 12)
            {
                dayUpdate = true;
            }*/

            for (var i = 0; i < timeSeries.Count; i++)
            {
                var currentWeather = timeSeries[i];
                var now = currentWeather.Time.TimeOfDay;
                var dateIsToday = currentWeather.Time.Date == DateTime.Today.Date;
                var dateIsTomorrow = currentWeather.Time.Date == DateTime.Today.AddDays(1).Date;
                var addEntityToList = false;

                if (nightUpdate && dateIsToday && InsideTimeSpan(new TimeSpan(0, 30, 0), new TimeSpan(12, 30, 0), now)) addEntityToList = true;
                if (dayUpdate && dateIsToday && InsideTimeSpan(new TimeSpan(12, 30, 0), new TimeSpan(23, 30, 0), now)) addEntityToList = true;
                if (dayUpdate && dateIsTomorrow && currentWeather.Time.Hour == 0) addEntityToList = true;

                if (addEntityToList)
                {
                    var instantWeatherDetails = currentWeather.Data.Instant.Details;
                    var next1HoursWeatherDetails = currentWeather.Data.Next_1_Hours.Details;

                    logger.LogInformation("Converting weather from model to entity...");
                    logger.LogInformation("Wind speed: " + instantWeatherDetails.WindSpeed.ToString());
                    logger.LogInformation("Wind direction: " + instantWeatherDetails.WindFromDirection.ToString());
                    logger.LogInformation("Rain: " + next1HoursWeatherDetails.PrecipitationAmount.ToString());
                    logger.LogInformation("Humidity: " + instantWeatherDetails.RelativeHumidity.ToString());
                    logger.LogInformation("Pressure: " + instantWeatherDetails.AirPressureAtSeaLevel.ToString());
                    logger.LogInformation("Cloud fraction: " + instantWeatherDetails.CloudAreaFraction.ToString();
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
                    });
                    logger.LogInformation("Successfully converted from model to entity...");
                }
            }
            return weatherList;

        }

        private static bool InsideTimeSpan(TimeSpan start, TimeSpan end, TimeSpan now)
        {
            if ((now > start) && (now < end))
            {
                return true;
            }
            return false;
        }
    }
}
