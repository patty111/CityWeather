using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CityWeather.Controllers.WeatherController
{
    [ApiController]
    [Route("weathersearch/show")]
    public class ShowWeatherController : Controller
    {
        private readonly IConfiguration _configuration;

        public ShowWeatherController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult showTable()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            string query = "SELECT * FROM weather";

            using SqlConnection connection = new SqlConnection(connectionString);
            {
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);
                using SqlDataReader reader = command.ExecuteReader();

                var result = new List<Dictionary<string, object>>();

                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);
                        object columnValue = reader.GetValue(i);
                        row[columnName] = columnValue;
                    }
                    result.Add(row);
                }

                return Json(result);
            }
        }
    }
}
