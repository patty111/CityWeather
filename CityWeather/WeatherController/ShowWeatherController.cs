using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace CityWeather.WeatherController
{
    [ApiController]
    [Route("weathersearch")]
    public class ShowWeatherController : Controller
    {
        [HttpPatch]
        public IActionResult showTable(bool? show)
        {
            if (show == true)
            {
                String connectionString = "Data Source=.\\SQLExpress;Initial Catalog=dev-test;Integrated Security=True";
                String query = "SELECT * FROM weather";
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
            return Ok();
        }
    }
}
