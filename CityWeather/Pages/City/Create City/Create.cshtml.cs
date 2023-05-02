using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CityWeather.Pages.City.Create_City
{
    public class CreateModel : PageModel
    {
        public CityInfo cityInfo = new CityInfo();
        public String errorMsg = "";
        public String successMsg = "";
        public void OnGet()
        {

        }

        public void OnPost() 
        {
            cityInfo.cityname = Request.Form["cityname"];
			cityInfo.latitude = Request.Form["latitude"];
			cityInfo.longtitude = Request.Form["longtitude"];
			cityInfo.last_modify = DateTime.Now.ToString();
		
            
            if (cityInfo.cityname.Length == 0 || cityInfo.longtitude.Length == 0
                || cityInfo.latitude.Length == 0)
            {
                errorMsg = "All fields must be filled";
                return;
            }
        }

        
	}
}
