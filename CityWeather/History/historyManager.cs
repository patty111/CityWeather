using CityWeather.CityController;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CityWeather.History
{
    public record HistoryObject
    {
        public String cityname;
        public String latitude;
        public String longitude;
        public String temperature;
        public String last_modify;
        public String search_time;
    }

    public class HistoryManager
    {
        readonly string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                "Integrated Security=True";
        public void HistoryAdd(CityDTO City, WeatherDTO Weather)
        {

            string query = "INSERT INTO history" +
                "(cityname, latitude, longitude, temperature, last_modify, search_time) VALUES " +
                "(@cityname, @latitude, @longitude, @temperature, @last_modify, @search_time)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using SqlCommand command = new SqlCommand(query, connection);
                Console.WriteLine(Weather.LastModified);

                command.Parameters.AddWithValue("@cityname", City.CityName);
                command.Parameters.AddWithValue("@latitude", City.Latitude != null ? City.Latitude : DBNull.Value);
                command.Parameters.AddWithValue("@longitude", City.Longitude != null ? City.Longitude : DBNull.Value);
                command.Parameters.AddWithValue("@temperature", Weather.Temperature != null ? Weather.Temperature : DBNull.Value);
                command.Parameters.AddWithValue("@last_modify", Weather.LastModified != null ? Weather.LastModified : DBNull.Value);
                command.Parameters.AddWithValue("@search_time", DateTime.Now);
                command.ExecuteNonQuery();
            }
        }



        public List<HistoryObject> HistoryPeek(int retrieve_nums)
        {
            string query = "SELECT TOP (@x) * FROM history ORDER BY search_time DESC";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@x", retrieve_nums);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        List<HistoryObject> historyObjects = new List<HistoryObject>();

                        while (reader.Read())
                        {
                            HistoryObject historyObject = new HistoryObject
                            {
                                cityname = reader.GetSqlString(0).ToString(),
                                latitude = reader.GetSqlString(1).ToString(),
                                longitude = reader.GetSqlString(2).ToString(),
                                temperature = reader.GetSqlDecimal(3).ToString(),
                                last_modify = reader.GetSqlDateTime(4).ToString(),
                                search_time = reader.GetSqlDateTime(5).ToString(),
                            };

                            historyObjects.Add(historyObject);
                        }
                        return historyObjects;
                    }
                }
            }
        }


        public int HistoryDelete(int? delete_nums, int? minutes)
        {
            String query = "";
            int flag = 0;
            if (delete_nums.HasValue)
            {
                flag = 1;
                query = $"DELETE FROM history WHERE search_time IN " +
                         $"(SELECT TOP (@n) search_time FROM history ORDER BY search_time DESC)";
            }
            else if (minutes.HasValue)
            {
                flag = 2;
                query = "DELETE FROM history WHERE search_time >= @startTime AND search_time <= @endTime";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (flag == 1)
                    {
                        command.Parameters.AddWithValue("@n", delete_nums);
                    }

                    if (flag == 2)
                    {
                        command.Parameters.AddWithValue("@startTime", DateTime.Now.AddMinutes(-(double)minutes));
                        command.Parameters.AddWithValue("@endTime", DateTime.Now);
                    }

                    int rows_effected = command.ExecuteNonQuery();
                    return rows_effected;
                }
            }
        }






    }
}
