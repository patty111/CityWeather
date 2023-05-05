
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using CityWeather.CityController;

namespace CityWeather.Pages.Search_History
{
    public class HistoryPageModel : PageModel
    {
        public List<SearchInfo> ListSearch = new();
		public void OnGet()
		{
			try
			{
				String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;Integrated Security=True";
				using (SqlConnection connection = new(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM history ORDER BY search_time DESC";
					using (SqlCommand command = new(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							int row_num = 1;

							while (reader.Read())
							{
								SearchInfo searchInfo = new();
								searchInfo.num = row_num.ToString();
								searchInfo.cityname = reader.GetString(0);
								searchInfo.latitude = reader["latitude"] == DBNull.Value ? "-" : reader["latitude"].ToString();
								searchInfo.longitude = reader["longitude"] == DBNull.Value ? "-" : reader["longitude"].ToString();
								searchInfo.temperature = reader["temperature"] == DBNull.Value ? "-" : (reader.GetSqlDecimal(3) - 273.15m).ToString();
								searchInfo.last_modify = reader["last_modify"] == DBNull.Value ? "-" : reader["last_modify"].ToString();
								searchInfo.search_time = reader.GetSqlDateTime(5).ToString();
								
								ListSearch.Add(searchInfo);
								row_num++;
							}
						}

					}

				}

			}

			catch (Exception ex)
			{
				Console.WriteLine("Exception: ", ex.Message);
			}
			return;
		}

	}

	public class SearchInfo
	{
		public String num;
		public String cityname;
		public String latitude;
		public String longitude;
		public String temperature;
		public String last_modify;
		public String search_time;
	}
}
