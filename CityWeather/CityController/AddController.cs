using CityWeather.CityController;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CityWeather.CityContoller;

[ApiController]
[Route("citydata")]
public class AddController : Controller
{
    [HttpPost()]
    public IActionResult CreateCity(String cityname, decimal latitude, decimal longitude)
    {
        String errorMsg = "";
        CityDTO City = new CityDTO();
        WeatherDTO Weather = new WeatherDTO();

        City.Id = Guid.NewGuid();
        City.CityName = cityname;
        City.Latitude = latitude;
        City.Longitude = longitude;
        Weather.Temperature = 0;
        Weather.LastModified = DateTime.Now;
        
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


                String query = "INSERT INTO Cities" +
                    "(id, cityname, latitude, longitude, temperature, last_modify) VALUES " +
                    "(@id, @cityname, @latitude, @longitude, @temperature, @last_modify)";


                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", City.Id);
                    command.Parameters.AddWithValue("@cityname", City.CityName);
                    command.Parameters.AddWithValue("@latitude", City.Latitude);
                    command.Parameters.AddWithValue("@longitude", City.Longitude);
                    command.Parameters.AddWithValue("@temperature", 0);
                    command.Parameters.AddWithValue("@last_modify", Weather.LastModified);

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
            City = City,
            Weather = Weather
        });
    }
}