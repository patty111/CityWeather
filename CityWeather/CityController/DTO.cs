namespace CityWeather.CityController
{
    public class CityDTO
    {
        public Guid Id { get; set; }
        public string CityName { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }


    public class WeatherDTO
    {
        public decimal? Temperature { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
