using Microsoft.Azure.Cosmos.Table;
using System;

namespace WeatherCollector.Repositories.Entities
{
    public class WeatherEntity : TableEntity
    {
/*        public WeatherEntity(string partitionKey)
        {
            // PartitionKey = partitionKey;
        }*/

        public DateTime Time { get; set; }
        public string Location { get; set; }
        public double Temperature { get; set; }
        public double RainNext6hrs { get; set; }
        public double WindSpeed { get; set; }
        public string WindDirection { get; set; }
        public double Cloudfraction { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }
    }
}
