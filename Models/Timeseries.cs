using System;

namespace WeatherCollector.Models
{
    public class Timeseries
    {
        public DateTime Time { get; set; }
        public Data Data { get; set; }
    }
}