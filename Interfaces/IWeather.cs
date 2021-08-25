using WeatherCollector.Models;

namespace WeatherCollector.Interfaces
{
    public interface IWeather
    {
        string Type { get; set; }
        Geometry Geometry { get; set; }
        Properties Properties { get; set; }
    }
}
