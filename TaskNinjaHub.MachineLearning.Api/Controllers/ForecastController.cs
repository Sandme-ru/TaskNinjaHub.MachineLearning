using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.MachineLearning.Api.Models;
using TaskNinjaHub.MachineLearning.Application;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;

namespace TaskNinjaHub.MachineLearning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController(TrainingCore trainingCore) : ControllerBase
{
    private const string TrainedModelKeras = "trained_model.keras";

    [HttpPost("Train")]
    public IActionResult Train([FromBody] List<CatalogTask> tasks)
    {
        trainingCore.TrainAndSaveModel(tasks);
        return Ok();
    }

    [HttpPost("PredictProbability")]
    public IActionResult PredictProbability([FromBody] TaskInputData inputData)
    {
        var probability = trainingCore.PredictProbability(inputData.PriorityId, inputData.InformationSystemId, inputData.TaskExecutorId, TrainedModelKeras);
        return Ok(probability);
    }
}
