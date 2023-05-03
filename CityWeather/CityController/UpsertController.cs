using City.Contract;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace CityWeather.CityController
{
    [ApiController]
    [Route("citydata")]
    public class UpsertController : Controller
    {
        [HttpPut()]
        public IActionResult UpsertCity(String cityname, Decimal latitude, Decimal longitude, Decimal temp)
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String upsertQuery = "UPDATE Cities " +
                        "SET latitude = @latitude, longitude = @longitude, temperature = @temp, " +
                        "last_modify = @last_modify " +
                        "OUTPUT INSERTED.id " +  // Retrieve the ID of the updated city
                        "WHERE cityname = @cityname";

                    using SqlCommand command = new SqlCommand(upsertQuery, connection);
                    {
                        DateTime last_modify = DateTime.Now;

                        command.Parameters.AddWithValue("@cityname", cityname);
                        command.Parameters.AddWithValue("@latitude", latitude);
                        command.Parameters.AddWithValue("@longitude", longitude);
                        command.Parameters.AddWithValue("@temp", temp);
                        command.Parameters.AddWithValue("@last_modify", last_modify);


                        object idResult = command.ExecuteScalar();
                        Guid cityId = idResult != null ? (Guid)idResult : Guid.Empty;

                        if (cityId != Guid.Empty)
                        {
                            return Ok(new
                            {
                                id = cityId,
                                cityname = cityname,
                                latitude = latitude,
                                longitude = longitude,
                                temperature = temp,
                                last_modify = last_modify.ToString(),
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
