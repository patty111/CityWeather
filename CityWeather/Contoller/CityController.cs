// http://localhost:5210/city/00000000-0000-0000-0000-000010012345
namespace City.Contoller;
using Microsoft.AspNetCore.Mvc;
using City.Contract;

[ApiController]
[Route("city")]
public class CityController : ControllerBase
{
    [HttpPost()]
    public IActionResult CreateCity(CreateCityRequest request)
    {
        return Ok(request);
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetCity(Guid id)
    {
        return Ok(id);
    }


    [HttpPut("{id:guid}")]
    public IActionResult UpsertCity(Guid id, UpsertCityRequest request)
    {
        return Ok(request);
    }


    [HttpDelete("{id:guid}")]
    public IActionResult DeleteCity(Guid id)
    {
        return Ok(id);
    }

}