using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCollector.Interfaces;
using WeatherCollector.Repositories.Entities;
using WeatherCollector.Repositories.Interfaces;

namespace WeatherCollector.Repositories
{
    public class WeatherRepository : BaseRepository<WeatherEntity>, IWeatherRepository
    {
        const string TABLE_NAME = "weather";
        public WeatherRepository(string connectionString) : base(TABLE_NAME, connectionString) { }
        public async Task<IEnumerable<WeatherEntity>> AddOrUpdateAsync(ILogger logger, IWeather weather, string partitionKey)
        {
            try
            {
                var weatherEntityList = WeatherEntity.FromWeather(logger, weather, partitionKey);
                logger.LogInformation("WeatherEntityList count: " + weatherEntityList.Count);
                var resultList = new List<WeatherEntity>();
                for (var i = 0; i < weatherEntityList.Count; i++)
                {
                    logger.LogInformation("Writing new entity to db... ID: ", weatherEntityList[i].RowKey.ToString());
                    logger.LogInformation("Wind speed: " + weatherEntityList[i].WindSpeed.ToString());
                    logger.LogInformation("Wind direction: " + weatherEntityList[i].WindDirection.ToString());
                    logger.LogInformation("Rain: " + weatherEntityList[i].RainNext1hrs.ToString());
                    logger.LogInformation("Humidity: " + weatherEntityList[i].Humidity.ToString());
                    logger.LogInformation("Pressure: " + weatherEntityList[i].Pressure.ToString());
                    logger.LogInformation("Cloud fraction: " + weatherEntityList[i].CloudFraction.ToString());
                    var result = await base.AddOrUpdateAsync(weatherEntityList[i]);
                    resultList.Add(result);
                    logger.LogInformation("Sucessfully written to db!");
                }

                logger.LogInformation("New data succesfully saved to db!");
                return resultList;

            } 
            catch (Exception e)
            {
                logger.LogError("Error: ", e);
                logger.LogError("Stacktrace: ", e.StackTrace);
                throw;
            }
        }
    }
}
