using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;
using CityWeather.History;
using CityWeather.Models;
using Microsoft.Extensions.Configuration;

namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]
    public class SearchController : ControllerBase
    {
        private readonly IConfiguration configuration;
        private readonly HistoryManager historyManager;

        public SearchController(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.historyManager = new HistoryManager(configuration);
        }

        [HttpGet("{cityname}")]
        public IActionResult SearchCity(string cityname)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Cities WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@cityname", cityname);

                    CityDTO city = new CityDTO();
                    WeatherDTO weather = new WeatherDTO();
                    city.CityName = cityname;

                    using SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        city.Id = reader.GetGuid(0);
                        city.CityName = reader.GetString(1);
                        city.Latitude = reader.GetDecimal(2);
                        city.Longitude = reader.GetDecimal(3);

                        weather.Temperature = reader.GetDecimal(4);
                        weather.LastModified = reader.GetDateTime(5);

                        historyManager.HistoryAdd(city, weather);

                        return Ok(new
                        {
                            City = city,
                            Weather = weather
                        });
                    }

                    weather.LastModified = null;
                    historyManager.HistoryAdd(city, weather);
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
