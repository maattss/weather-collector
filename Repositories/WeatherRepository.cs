using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherCollector.Repositories.Entities;

namespace WeatherCollector.Repositories
{
    public class WeatherRepository : BaseRepository<WeatherEntity>
    {
        const string TABLE_NAME = "weather";
        private string connectionString;
        public WeatherRepository(string connectionString) : base(TABLE_NAME, connectionString)
        {
            this.connectionString = connectionString;
        }
    }
}
