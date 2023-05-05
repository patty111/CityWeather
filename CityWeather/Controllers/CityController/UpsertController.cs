using CityWeather.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;

namespace CityWeather.Controllers.CityController
{
    [ApiController]
    [Route("citydata")]
    public class UpsertController : BaseController
    {
        private readonly SqlConnection _connection;

        public UpsertController(IConfiguration configuration) : base(configuration)
        {
            _connection = new SqlConnection(Configuration.GetConnectionString("DefaultConnection"));
        }

        [HttpPut()]
        public IActionResult UpsertCity(string cityname, decimal latitude, decimal longitude, decimal temp)
        {
            CityDTO city = new CityDTO();
            WeatherDTO weather = new WeatherDTO();
            city.CityName = cityname;
            city.Latitude = latitude;
            city.Longitude = longitude;
            weather.Temperature = temp;
            weather.LastModified = DateTime.Now;

            try
            {
                _connection.Open();

                string upsertQuery = "UPDATE Cities " +
                    "SET latitude = @latitude, longitude = @longitude, temperature = @temp, " +
                    "last_modify = @last_modify " +
                    "OUTPUT INSERTED.id " +  // Retrieve the ID of the updated city
                    "WHERE cityname = @cityname";

                using (SqlCommand command = new SqlCommand(upsertQuery, _connection))
                {
                    command.Parameters.AddWithValue("@cityname", city.CityName);
                    command.Parameters.AddWithValue("@latitude", city.Latitude);
                    command.Parameters.AddWithValue("@longitude", city.Longitude);
                    command.Parameters.AddWithValue("@temp", weather.Temperature);
                    command.Parameters.AddWithValue("@last_modify", weather.LastModified);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        city.Id = (Guid)command.ExecuteScalar();

                        return Ok(new
                        {
                            City = city,
                            Weather = weather,
                        });
                    }

                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
