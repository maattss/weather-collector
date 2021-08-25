using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCollector.Interfaces;
using WeatherCollector.Repositories.Entities;

namespace WeatherCollector.Repositories
{
    public class WeatherRepository : BaseRepository<WeatherEntity>
    {
        const string TABLE_NAME = "weather";
        private string connectionString;
        public WeatherRepository(string connectionString) : base(TABLE_NAME, connectionString)
        {
            this.connectionString = connectionString;
        }
        public async Task<List<WeatherEntity>> AddOrUpdateAsync(ILogger logger, IWeather weather, string partitionKey, bool earlyUpdate)
        {
            try
            {
                var weatherEntityList = WeatherEntity.FromWeather(weather, partitionKey, earlyUpdate);
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
