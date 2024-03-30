using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.MachineLearning.Application;

namespace TaskNinjaHub.MachineLearning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok(new Core().Main());
    }
}