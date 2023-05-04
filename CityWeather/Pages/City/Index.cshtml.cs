using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CityWeather.Pages.City
{
    public class IndexModel : PageModel
    {
        public List<CityInfo> ListCity = new();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;Integrated Security=True";
                using (SqlConnection connection = new(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Cities";
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
		public String num;
		public String cityname;
		public String latitude;
		public String longitude;
        public String temperature;
		public String last_modify;
		public String id;

	}
}
