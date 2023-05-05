using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CityWeather.WeatherController
{

    [ApiController]
    [Route("weathersearch")]
    public class WeatherSearchController : Controller
    {
        String connectionString = "Data Source=.\\SQLExpress;Initial Catalog=dev-test;Integrated Security=True";
        
        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new();
        public WeatherSearchController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [HttpGet]
        public async Task<IActionResult> WSearch(double lat, double lon)
        {
            try
            {
                String apiKey = _configuration["WeatherAPIKey"];
                String url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
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



                using SqlConnection connection = new SqlConnection(connectionString);
                {
                    connection.Open();

                    String query = "INSERT INTO weather" +
                    "(search_time, latitude, longitude, temperature, descript) VALUES " +
                    "(@search_time, @latitude, @longitude, @temperature, @descript)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@search_time", DateTime.Now);
                        command.Parameters.AddWithValue("@latitude", searchResponse.latitude);
                        command.Parameters.AddWithValue("@longitude", searchResponse.longitude);
                        command.Parameters.AddWithValue("@temperature", searchResponse.temperature);
                        command.Parameters.AddWithValue("@descript", searchResponse.description);
                        command.ExecuteNonQuery();
                    }
                }


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
