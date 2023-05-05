using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using CityWeather.Models;

namespace CityWeather.Controllers.CityController
{
    [ApiController]
    [Route("citydata")]
    public class AddController : BaseController
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public AddController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<decimal> GetTemp(decimal lat, decimal lon)
        {
            try
            {
                string apiKey = Configuration["APISettings:apiKey"];
                string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
                Uri uri = new Uri(url);

                using HttpResponseMessage response = await _httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();
                dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
                return jsonObject.main.temp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        [HttpPost()]
        public async Task<IActionResult> CreateCity(string cityname, decimal latitude, decimal longitude)
        {
            string errorMsg = "";
            CityDTO City = new CityDTO();
            WeatherDTO Weather = new WeatherDTO();

            City.Id = Guid.NewGuid();
            City.CityName = cityname;
            City.Latitude = latitude;
            City.Longitude = longitude;
            Weather.Temperature = await GetTemp(latitude, longitude);
            Weather.LastModified = DateTime.Now;

            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM Cities WHERE cityname = @cityname";
                    using SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                    checkCommand.Parameters.AddWithValue("@cityname", cityname);

                    int existingCount = (int)checkCommand.ExecuteScalar();
                    if (existingCount > 0)
                    {
                        errorMsg = "City already exists in the database.";
                        return Conflict(errorMsg); // Return 409 Conflict status code
                    }

                    string query = "INSERT INTO Cities " +
                        "(id, cityname, latitude, longitude, temperature, last_modify) VALUES " +
                        "(@id, @cityname, @latitude, @longitude, @temperature, @last_modify)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", City.Id);
                        command.Parameters.AddWithValue("@cityname", City.CityName);
                        command.Parameters.AddWithValue("@latitude", City.Latitude);
                        command.Parameters.AddWithValue("@longitude", City.Longitude);
                        command.Parameters.AddWithValue("@temperature", Weather.Temperature);
                        command.Parameters.AddWithValue("@last_modify", Weather.LastModified);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return Created("/citydata", new
            {
                City,
                Weather
            });
        }
    }
}
