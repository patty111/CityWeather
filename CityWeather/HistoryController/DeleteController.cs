using CityWeather.History;
using Microsoft.AspNetCore.Mvc;

namespace CityWeather.HistoryController
{
    [ApiController]
    [Route("historydata")]
    public class DeleteController : Controller
    {
        [HttpDelete]
        public IActionResult Delete(int? delete_nums = null, int? minutes = null)
        {
            HistoryManager historyManager = new HistoryManager();
            int effected_rows = historyManager.HistoryDelete(delete_nums, minutes);
            return Ok(new
            {
                Msg = effected_rows.ToString() + " deleted"
            });
        }
    }
}
