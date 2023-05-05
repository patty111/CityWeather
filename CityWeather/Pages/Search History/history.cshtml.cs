using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace CityWeather.Pages.Search_History
{
    public class HistoryPageModel : PageModel
    {
        private readonly IConfiguration configuration;

        public List<SearchInfo> ListSearch { get; set; }

        public HistoryPageModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void OnGet()
        {
            try
            {
                string connectionString = configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM history ORDER BY search_time DESC";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        ListSearch = new List<SearchInfo>();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            int row_num = 1;

                            while (reader.Read())
                            {
                                SearchInfo searchInfo = new SearchInfo();
                                searchInfo.num = row_num.ToString();
                                searchInfo.cityname = reader.GetString(0);
                                searchInfo.latitude = reader["latitude"] == DBNull.Value ? "-" : reader["latitude"].ToString();
                                searchInfo.longitude = reader["longitude"] == DBNull.Value ? "-" : reader["longitude"].ToString();
                                searchInfo.temperature = reader["temperature"] == DBNull.Value ? "-" : ((reader.GetSqlDecimal(3) - 273.15m).ToString());
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
                Console.WriteLine("Exception: " + ex.Message);
            }
        }
    }

    public class SearchInfo
    {
        public string num;
        public string cityname;
        public string latitude;
        public string longitude;
        public string temperature;
        public string last_modify;
        public string search_time;
    }
}
