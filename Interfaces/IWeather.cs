using WeatherCollector3.Models;

namespace WeatherCollector3.Interfaces
{
    public interface IWeather
    {
        string Type { get; set; }
        Geometry Geometry { get; set; }
        Properties Properties { get; set; }
    }
}
