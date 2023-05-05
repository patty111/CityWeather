using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace CityWeather.Controllers.CityController
{
    [ApiController]
    [Route("citydata")]

    public class DeleteController : BaseController
    {
        public DeleteController(IConfiguration configuration) : base(configuration)
        {
        }

        [HttpDelete("{cityname}")]
        public IActionResult DeleteCity(string cityname)
        {
            try
            {
                string connectionString = Configuration.GetConnectionString("DefaultConnection");
                string apiKey = Configuration["APISettings:ApiKey"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM Cities WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(deleteQuery, connection);
                    {
                        command.Parameters.AddWithValue("@cityname", cityname);
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return NoContent();
                        }
                        else
                        {
                            return NotFound();
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