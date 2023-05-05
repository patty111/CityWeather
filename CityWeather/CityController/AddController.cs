using CityWeather.CityController;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace CityWeather.CityContoller;

[ApiController]
[Route("citydata")]
public class AddController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IConfiguration _configuration;
    private static readonly HttpClient _httpClient = new();

    public AddController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<Decimal> GetTemp(Decimal lat, Decimal lon)
    {
        try
        {
            String apiKey = _configuration["ApiSettings:ApiKey"];
            String url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
            String apiKey = _configuration["ApiSettings:ApiKey"];
            String url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&appid={apiKey}";
            Uri uri = new Uri(url);

            using HttpResponseMessage response = await _httpClient.GetAsync(uri);
            Console.WriteLine(response.EnsureSuccessStatusCode().ToString());

            String jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic jsonObject = JsonConvert.DeserializeObject(jsonResponse);
            return jsonObject.main.temp;
        }
        catch (Exception ex)
        {
            return 0;
        }
    }


    [HttpPost()]
    public async Task<IActionResult> CreateCity(String cityname, decimal latitude, decimal longitude)
    {
        String errorMsg = "";
        CityDTO City = new CityDTO();
        WeatherDTO Weather = new WeatherDTO();

        City.Id = Guid.NewGuid();
        City.CityName = cityname;
        City.Latitude = latitude;
        City.Longitude = longitude;
        Weather.Temperature = await GetTemp(latitude, longitude);
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
                    command.Parameters.AddWithValue("@temperature", Weather.Temperature);
                    command.Parameters.AddWithValue("@last_modify", Weather.LastModified);

                    command.ExecuteNonQuery();
                }
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }

        return Created("/citydata", new
        {
            City = City,
            Weather = Weather
        });
    }
}
