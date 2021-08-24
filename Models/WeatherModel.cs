using System.Collections.Generic;
using WeatherCollector3.Interfaces;

namespace WeatherCollector3.Models
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
