using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCollector.Interfaces;
using WeatherCollector.Repositories.Entities;

namespace WeatherCollector.Repositories.Interfaces
{
    interface IWeatherRepository
    {
        Task<IEnumerable<WeatherEntity>> AddOrUpdateAsync(ILogger logger, IWeather weather, string partitionKey, bool earlyUpdate);
        // Task<IWeather> GetWeatherAsync(ILogger logger, string rowKey, string partitionKey);
        // Task<IEnumerable<WeatherEntity>> GetAllWeather(ILogger logger, string rowKey);
    }
}
