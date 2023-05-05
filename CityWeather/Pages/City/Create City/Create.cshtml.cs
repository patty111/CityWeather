using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace CityWeather.Pages.City.Create_City
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public CityInfo cityInfo = new CityInfo();
        public string errorMsg = "";
        public string successMsg = "";

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async void OnPost()
        {
            cityInfo.id = Guid.NewGuid().ToString();
            cityInfo.cityname = Request.Form["cityname"];
            cityInfo.latitude = Request.Form["latitude"];
            cityInfo.longitude = Request.Form["longitude"];

            //  cityInfo.temperature = await GetWeather.GetTempData(cityInfo.cityname);

            // cityInfo.last_modify = DateTime.Now.ToString();

            bool validLatitude = Regex.IsMatch(cityInfo.latitude, @"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?)$");
            bool validLongitude = Regex.IsMatch(cityInfo.longitude, @"^[-+]?((1[0-7]|[1-9])?\d(\.\d+)?|180(\.0+)?)$");

            if (string.IsNullOrEmpty(cityInfo.cityname) || string.IsNullOrEmpty(cityInfo.latitude) || string.IsNullOrEmpty(cityInfo.longitude))
            {
                errorMsg = "All fields must be filled";
                return;
            }

            else if (!validLatitude)
            {
                errorMsg = "Invalid latitude input";
                return;
            }

            else if (!validLongitude)
            {
                errorMsg = "Invalid longitude input";
                return;
            }

            // save to database
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                using SqlConnection connection = new SqlConnection(connectionString);
                {
                    connection.Open();
                    string add = "INSERT INTO Cities(id, cityname, latitude, longitude, temperature, last_modify) VALUES (@id, @cityname, @latitude, @longitude, @temperature, @last_modify)";

                    using SqlCommand command = new SqlCommand(add, connection);
                    {
                        command.Parameters.AddWithValue("@id", cityInfo.id);
                        command.Parameters.AddWithValue("@cityname", cityInfo.cityname);
                        command.Parameters.AddWithValue("@latitude", cityInfo.latitude);
                        command.Parameters.AddWithValue("@longitude", cityInfo.longitude);
                        command.Parameters.AddWithValue("@temperature", 0);
                        command.Parameters.AddWithValue("@last_modify", DateTime.Now);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }

            cityInfo.cityname = "";
            cityInfo.latitude = "";
            cityInfo.longitude = "";
            cityInfo.id = "";
            cityInfo.last_modify = "";
            cityInfo.temperature = "";

            successMsg = "City successfully created";
            // Response.Redirect("../../City");
            return;
        }
    }
}
