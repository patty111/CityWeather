using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]

    public class DeleteController : Controller
    {
        [HttpDelete()]
        public IActionResult DeleteCity([FromQuery]String cityname)
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String deleteQuery = "DELETE FROM Cities WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(deleteQuery, connection);
                    {
                        command.Parameters.AddWithValue("@cityname", cityname);
                        int rows_affected = command.ExecuteNonQuery();
                        if (rows_affected > 0)
                        {
                            return Ok(new
                            {
                                Msg = "City successfully deleted"
                            });
                        }
                        else
                        {
                            return NotFound(new
                            {
                                Msg = "City not found",
                                status = "404"
                            });
                        }

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
