using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CityWeather.WeatherController
{
    [ApiController]
    [Route("weathersearch")]
    public class WeatherSearchController : Controller
    {
        private static readonly HttpClient _httpClient = new();
        private static readonly String _apiKey = "8cb8460525cde5bbc3891e8f3e150bfc";

        [HttpGet]
        public async Task<IActionResult> WSearch(double lat, double lon)
        {
            try
            {
                String url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={_apiKey}";
                Uri uri = new Uri(url);


                using HttpResponseMessage response = await _httpClient.GetAsync(uri);

                String jsonResponse = await response.Content.ReadAsStringAsync();
                var jsonObject = JsonConvert.DeserializeObject<dynamic>(jsonResponse);


                SearchResponse searchResponse = new SearchResponse()
                {
                    country = jsonObject.sys.country,
                    cityname = jsonObject.name,
                    latitude = jsonObject.coord.lat.ToString(),
                    longitude = jsonObject.coord.lon.ToString(),
                    temperature = jsonObject.main.temp.ToString(),
                    description = jsonObject.weather[0].description,

                };

                return Ok(JsonConvert.SerializeObject(searchResponse));
            }
            
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private class SearchResponse
        {
            public String country;
            public String cityname;
            public String latitude;
            public String longitude;
            public String temperature;
            public String description;
        }
    }
}
