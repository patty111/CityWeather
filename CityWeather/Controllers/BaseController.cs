using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CityWeather.Controllers
{
    public class BaseController : ControllerBase
    {
        protected readonly IConfiguration Configuration;

        public BaseController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}