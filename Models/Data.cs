namespace WeatherCollector.Models
{
    public class Data
    {
        public Instant Instant { get; set; }
        public Next1Hours Next_1_Hours { get; set; }
        public Next6Hours Next_6_Hours { get; set; }
        public Next12Hours Next_12_Hours { get; set; }    
    }

    public class Instant
    {
        public Details Details { get; set; }
    }

    public class Next1Hours
    {
        public Summary Summary { get; set; }
        public ForecastDetails Details { get; set; }
    }

    public class Next6Hours
    {
        public Summary Summary { get; set; }
        public ForecastDetails Details { get; set; }
    }
    public class Next12Hours
    {
        public Summary Summary { get; set; }
    }

    public class Summary
    {
        public string SymbolCode { get; set; }
    }
    public class ForecastDetails
    {
        public double PrecipitationAmount { get; set; }
    }
}