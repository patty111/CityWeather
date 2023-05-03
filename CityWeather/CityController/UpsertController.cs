using Microsoft.AspNetCore.Mvc;
using System;
using System.Data.SqlClient;

namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]
    public class UpsertController : Controller
    {
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
                string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string upsertQuery = "UPDATE Cities " +
                        "SET latitude = @latitude, longitude = @longitude, temperature = @temp, " +
                        "last_modify = @last_modify " +
                        "OUTPUT INSERTED.id " +  // Retrieve the ID of the updated city
                        "WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(upsertQuery, connection);
                    {
                        command.Parameters.AddWithValue("@cityname", city.CityName);
                        command.Parameters.AddWithValue("@latitude", city.Latitude);
                        command.Parameters.AddWithValue("@longitude", city.Longitude);
                        command.Parameters.AddWithValue("@temp", weather.Temperature);
                        command.Parameters.AddWithValue("@last_modify", weather.LastModified);

                        object idResult = command.ExecuteScalar();
                        Guid cityId = idResult != null ? (Guid)idResult : Guid.Empty;

                        if (cityId != Guid.Empty)
                        {
                            city.Id = cityId;

                            return Ok(new
                            {
                                City = city,
                                Weather = weather,
                            });
                        }

                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
