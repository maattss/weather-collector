using System.Collections.Generic;
using WeatherCollector.Interfaces;

namespace WeatherCollector.Models
{
    public class WeatherModel : IWeather
    {
        public string Type { get; set; }
        public Geometry Geometry { get; set; }
        public Properties Properties { get; set; }
    }

    public class Geometry
    {
        public string Type { get; set; }
        public List<double> Coordinates { get; set; }
    }

    public class Properties
    {
        public Meta Meta { get; set; }
        public List<Timeseries> Timeseries { get; set; }
    }
 
}
