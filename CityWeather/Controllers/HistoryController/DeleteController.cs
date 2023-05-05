using CityWeather.History;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CityWeather.Controllers.HistoryController
{
    [ApiController]
    [Route("historydata")]
    public class DeleteController : Controller
    {
        private readonly IConfiguration configuration;

        public DeleteController(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [HttpDelete]
        public IActionResult Delete(int? delete_nums = null, int? minutes = null)
        {
            HistoryManager historyManager = new HistoryManager(configuration);
            int effected_rows = historyManager.HistoryDelete(delete_nums, minutes);
            return Ok(new
            {
                Msg = effected_rows.ToString() + " deleted"
            });
        }
    }
}
