using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WeatherCollector.Repositories;
using WeatherCollector.Repositories.Interfaces;

namespace WeatherCollector
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults()
                .ConfigureServices(s =>
                {
                    s.AddScoped<IWeatherRepository, WeatherRepository>();
                })
                .Build();

            host.Run();
        }
    }
}