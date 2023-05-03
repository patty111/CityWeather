using System;
using System.Net.Http;
using System.Threading.Tasks;
namespace CityWeather
{
	public class GetWeather
	{
		public static async Task<string> GetTempData(String cityname)
		{
			String apiKey = "8cb8460525cde5bbc3891e8f3e150bfc";
			String apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={cityname}&appid={apiKey}";

			using (HttpClient client = new HttpClient())
			{
				HttpResponseMessage response = await client.GetAsync(apiUrl);
				if (response.IsSuccessStatusCode)
				{
					string data = await response.Content.ReadAsStringAsync();
					return data;
				}

				else
				{
					Console.WriteLine($"Error: {response.StatusCode}");
					return null;
				}
			}
		}
	}
}
