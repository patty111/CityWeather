using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CityWeather.Controllers.WeatherController
{

    [ApiController]
    [Route("weathersearch")]
    public class WeatherSearchController : BaseController
    {
        public WeatherSearchController(IConfiguration configuration) : base(configuration)
        {
        }

        private readonly IConfiguration _configuration;
        private static readonly HttpClient _httpClient = new();


        [HttpGet]
        public async Task<IActionResult> WSearch(double lat, double lon)
        {
            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");
                string apiKey = Configuration["APISettings:apiKey"];
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
                Uri uri = new Uri(url);


                using HttpResponseMessage response = await _httpClient.GetAsync(uri);

                string jsonResponse = await response.Content.ReadAsStringAsync();
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

                    string query = "INSERT INTO weather" +
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
            public string country;
            public string cityname;
            public string latitude;
            public string longitude;
            public string temperature;
            public string description;
        }
    }
}
