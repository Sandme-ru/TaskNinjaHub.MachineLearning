using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.MachineLearning.Application;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.MachineLearning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController(Core core) : ControllerBase
{
    [HttpGet]
    public IActionResult Test()
    {
        return Ok(core.Main());
    }

    [HttpPost("TestTasks")]
    public IActionResult TestTasks([FromBody] List<CatalogTask> tasks)
    {
        core.TrainAndSaveModel(tasks);
        return Ok();
    }
}