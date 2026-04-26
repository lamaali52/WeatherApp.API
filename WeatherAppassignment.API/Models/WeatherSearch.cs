namespace WeatherApp.API.Models
{
    public class WeatherSearch
    {
        public int SearchId { get; set; }
        public int UserId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }
        public DateTime SearchDate { get; set; }
        public string SearchedBy { get; set; } = string.Empty;
    }

    public class SaveWeatherRequest
    {
        public int UserId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public string CountryCode { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }
    }

    public class UpdateWeatherRequest
    {
        public int SearchId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int Humidity { get; set; }
        public decimal TempMin { get; set; }
        public decimal TempMax { get; set; }
        public int ChangedByUserId { get; set; }
    }
}