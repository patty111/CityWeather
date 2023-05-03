using Microsoft.AspNetCore.Mvc;
using City.Contract;
using System.Data.SqlClient;

namespace CityWeather.CityContoller;

[ApiController]
[Route("citydata")]
public class AddController : Controller
{
    [HttpPost("addcity")]
    public IActionResult CreateCity([FromQuery]String cityname, [FromQuery]decimal latitude, [FromQuery] decimal longitude)
    {
        String errorMsg = "";

        Guid id = Guid.NewGuid();
        String formattedLatitude = latitude.ToString();
        String formattedLongitude = longitude.ToString();
        DateTime lastModified = DateTime.Now;

        try
        {
            String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                "Integrated Security=True";
            using SqlConnection connection = new SqlConnection(connectionString);
            {
                connection.Open();

                String checkQuery = "SELECT COUNT(*) FROM Cities WHERE cityname = @cityname";
                using SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@cityname", cityname);

                int existingCount = (int)checkCommand.ExecuteScalar();
                if (existingCount > 0)
                {
                    errorMsg = "City already exists in the database.";
                    return Conflict(errorMsg); // Return 409 Conflict status code
                }


                String add = "INSERT INTO Cities" +
                    "(id, cityname, latitude, longitude, temperature, last_modify) VALUES " +
                    "(@id, @cityname, @latitude, @longitude, @temperature, @last_modify)";

                using (SqlCommand command = new SqlCommand(add, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@cityname", cityname);
                    command.Parameters.AddWithValue("@latitude", formattedLatitude);
                    command.Parameters.AddWithValue("@longitude", formattedLongitude);
                    command.Parameters.AddWithValue("@temperature", 0);
                    command.Parameters.AddWithValue("@last_modify", DateTime.Now);

                    command.ExecuteNonQuery();
                }
            }
        }

        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }


        return Ok(new
        {
            ID = id.ToString(),
            CityName = cityname,
            Latitude = formattedLatitude,
            Longitude = formattedLongitude,
            Temperature = "0",
            LastModified = lastModified.ToString(),
        });
    }







}