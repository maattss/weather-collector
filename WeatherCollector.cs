using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherCollector3.Models;

namespace WeatherCollector3
{
    public static class WeatherCollector
    {
        [Function("WeatherCollector")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2254:Template should be a static expression", Justification = "<Pending>")]
        public static async Task RunAsync([TimerTrigger("*/10 * * * * *")] FunctionContext context)
        {
            var logger = context.GetLogger("WeatherCollector");
            logger.LogInformation($"Weather Collector function executed at: {DateTime.Now}");
            // Fetch current weather data from from Yr.no
            HttpClient client = new();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/1.0)");
            var uri = "https://api.met.no/weatherapi/locationforecast/2.0/compact";
            var HomeParameters = "?altitude=8&lat=58.93&lon=5.69";
            // var BergenParameters = "?altitude=650&lat=58.68&lon=7.05";
            // var KnabenParameters = "?altitude=100&lat=60.33&lon=5.29";
            var contentReq = await client.GetAsync(uri + HomeParameters);
            var contentAsString = await contentReq.Content.ReadAsStringAsync();
            var contentObj = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherModel>(contentAsString);
            logger.LogInformation("Content object time:");
            logger.LogInformation(contentObj.Properties.Timeseries[0].Time.ToString());
            // logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");

            // Save to db

          /*  var connectionString = "DefaultEndpointsProtocol=https;AccountName=weather-storage-dev;AccountKey=pQl3oJu6IuTcVOAks96nup7VtCjEIcmxxNAlHT2B4jDT4dBnrMF9mL02SBNTK6oBu1EgYQy90Tpb7jk1X2ZIpQ==;TableEndpoint=https://weather-storage-dev.table.cosmos.azure.com:443/;";
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            await tableClient.GetTableReference("Test").CreateIfNotExistsAsync();
            await tableClient.GetTableReference("Test").ExecuteAsync(TableOperation.Insert(new TestEntity("Testing")));*/
        }
    }
}
