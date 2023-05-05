using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CityWeather.Pages.City
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration configuration;

        public IndexModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public List<CityInfo> ListCity = new();

        public void OnGet()
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Cities ORDER BY cityname ASC";
                    using (SqlCommand command = new(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var row_num = 1;
                            while (reader.Read())
                            {
                                CityInfo cityInfo = new()
                                {
                                    num = row_num.ToString(),
                                    cityname = reader.GetString(1),
                                    latitude = "" + reader.GetSqlDecimal(2),
                                    longitude = "" + reader.GetSqlDecimal(3),
                                    temperature = "" + (reader.GetSqlDecimal(4).Value - 273.15m), // m specifies that it is type decimal
                                    last_modify = "" + reader.GetSqlDateTime(5),
                                    id = reader.GetSqlGuid(0).ToString()
                                };

                                ListCity.Add(cityInfo);
                                row_num++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: ", ex.ToString());
            }
            return;
        }
    }

    public class CityInfo
    {
        public string num;
        public string cityname;
        public string latitude;
        public string longitude;
        public string temperature;
        public string last_modify;
        public string id;
    }
}
