using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using CityWeather.History;

namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]
    public class SearchController : Controller
    {
        [HttpGet()]
        public IActionResult SearchCity(String cityname)
        {
            try
            {

                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";

                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    String query = "SELECT * FROM Cities WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@cityname", cityname);


                    HistoryManager history = new HistoryManager();

                    CityDTO city = new CityDTO();
                    WeatherDTO weather = new WeatherDTO();
                    city.CityName = cityname;

                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        
                        city.Id = reader.GetGuid(0);
                        city.CityName = reader.GetString(1);
                        city.Latitude = reader.GetDecimal(2);
                        city.Longitude = reader.GetDecimal(3);
                        
                        weather.Temperature = reader.GetDecimal(4);
                        weather.LastModified = reader.GetDateTime(5);
                        

                        history.HistoryAdd(city, weather);


                        return Ok(new
                        {
                            City = city,
                            Weather = weather
                        });
                    }
                    weather.LastModified = null;
                    history.HistoryAdd(city, weather);

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
