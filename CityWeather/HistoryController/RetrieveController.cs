using Microsoft.AspNetCore.Mvc;
using CityWeather.History;
namespace CityWeather.HistoryController
{
    [ApiController]
    [Route("historydata")]
    public class RetrieveController : Controller
    {
        [HttpGet()]
        public IActionResult Retrieve(int retrieve_nums)
        {
            HistoryManager historyManager = new HistoryManager();
            List<HistoryObject> historyObjects = historyManager.HistoryPeek(retrieve_nums);
            List<String> convert = new List<String>();

            foreach (HistoryObject historyObject in historyObjects)
            { 
                String tmp = historyObject.ToString();
                int startIdx = tmp.IndexOf("{") + 2;
                int endIdx = tmp.IndexOf("}");
                convert.Add(tmp.Substring(startIdx, endIdx - startIdx));
            }
            
            return Ok(convert);
        }
    }
}
