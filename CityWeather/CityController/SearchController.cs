using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]
    public class SearchController : Controller
    {
        [HttpGet()]
        public IActionResult SearchCity([FromQuery]String cityname)
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String query = "SELECT * FROM Cities WHERE cityname = @cityname";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cityname", cityname);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                String cityId = reader.GetSqlGuid(0).ToString();
                                String formatLatitude = reader.GetSqlDecimal(2).ToString();
                                String formatLongitude = reader.GetSqlDecimal(3).ToString();
                                String temp = (reader.GetSqlDecimal(4) - 273.15m).ToString();
                                String lastModify = reader.GetSqlDateTime(5).ToString();

                                return Ok(new
                                {
                                    id = cityId,
                                    cityname = reader.GetSqlString(1),
                                    latitude = formatLatitude,
                                    longitude = formatLongitude,
                                    temperature = temp,
                                    last_modify = lastModify
                                });
                            }
                        }
                    }
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
