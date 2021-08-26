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
        public async Task<IEnumerable<WeatherEntity>> AddOrUpdateAsync(ILogger logger, IWeather weather, string partitionKey, bool earlyUpdate)
        {
            try
            {
                logger.LogInformation("AddOrUpdateAsyncStart :)" + "\n" + weather + "\n" + partitionKey + "\n" + earlyUpdate);
                logger.LogInformation("Logger: " + "\n" + logger.ToString());
                logger.LogInformation("Weather: " + "\n" + weather.Properties.Timeseries[0].ToString());
                logger.LogInformation("Partitionkey: " + "\n" + partitionKey);
                logger.LogInformation("earlyUpdate: " + "\n" + earlyUpdate);
                var weatherEntityList = WeatherEntity.FromWeather(weather, partitionKey, earlyUpdate);
                logger.LogInformation("From weather result", weatherEntityList);
                var resultList = new List<WeatherEntity>();
                foreach (var weatherEntity in weatherEntityList)
                {
                    var result = await base.AddOrUpdateAsync(weatherEntity);
                    resultList.Add(result);
                }
                logger.LogInformation("New data succesfully saved to db!");
                return resultList;

            } 
            catch (Exception e)
            {
                logger.LogError("Error: ", e.Message);
                logger.LogError("Stacktrace: ", e.StackTrace);
                throw;
            }
        }
    }
}
