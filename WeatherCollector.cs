using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherCollector.Models;
using Microsoft.Azure.Cosmos.Table;
using WeatherCollector.Repositories;

namespace WeatherCollector
{
    public static class WeatherCollector
    {
        [Function("WeatherCollector")]
        public static async Task RunAsync([TimerTrigger("*/10 * * * * *")] FunctionContext context)
        {
            var logger = context.GetLogger("WeatherCollector");
            logger.LogInformation($"Weather Collector function executed at: {DateTime.Now}");
            
            // Fetch current weather data from from Yr.no
            HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
            var uri = "https://api.met.no/weatherapi/locationforecast/2.0/compact";
            var parameters = "?altitude=8&lat=58.93&lon=5.69";
            
            // var StavangerParameters = "?altitude=2&lat=58.97&lon=5.73";
            // var SandnesParameters = "?altitude=2&lat=58.85&lon=5.74";
            // var BergenParameters = "?altitude=650&lat=58.68&lon=7.05";
            // var KnabenParameters = "?altitude=100&lat=60.33&lon=5.29";
            
            var contentReq = await client.GetAsync(uri + parameters);
            var contentAsString = await contentReq.Content.ReadAsStringAsync();
            var contentObj = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(contentAsString);
            logger.LogInformation("Yr data updated at: " + contentObj.Properties.Meta.UpdatedAt.ToString());

            // Save to db
            logger.LogInformation("Saving new data to db...");
            var connStr = Environment.GetEnvironmentVariable("ConnectionStringCosmosDB");
            var weatherRepository = new WeatherRepository(connStr);
            await weatherRepository.AddOrUpdateAsync(logger, contentObj, "home", true);
        }
    }
}
