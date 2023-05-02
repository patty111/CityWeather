using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace CityWeather.Pages.City
{
    public class IndexModel : PageModel
    {
        public List<CityInfo> ListCity = new List<CityInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM Cities";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    { 
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            var row_num = 1;
                            while (reader.Read())
                            {
                                CityInfo cityInfo = new CityInfo();
                                cityInfo.num = row_num.ToString();
                                cityInfo.cityname = reader.GetString(1);
                                cityInfo.latitude = "" + reader.GetSqlDecimal(2);
                                cityInfo.longtitude = "" + reader.GetSqlDecimal(3);
                                cityInfo.last_modify = "" + reader.GetSqlDateTime(4);
                                cityInfo.id = reader.GetSqlGuid(0).ToString();
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
        }

        
	}

	public class CityInfo
	{
		public String num;
		public String cityname;
		public String latitude;
		public String longtitude;
		public String last_modify;
		public String id;

	}
}
