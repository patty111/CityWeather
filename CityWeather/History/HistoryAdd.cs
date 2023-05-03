using System.Data.SqlClient;
namespace CityWeather.History
{
    public class HistoryAdd
    {
        public HistoryAdd()
        {
            String connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=dev-test;" +
                    "Integrated Security=True";

            String query = "INSERT INTO history" +
                "(search_id, cityname, latitude, longitude, temperature, last_modify, search_time) VALUES " +
                "(@id, @cityname, @latitude, @longitude, @temperature, @last_modify, @search_time)";


        }
    }
}
