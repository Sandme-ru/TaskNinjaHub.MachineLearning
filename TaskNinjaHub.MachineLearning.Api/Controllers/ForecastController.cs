using Microsoft.AspNetCore.Mvc;
using TaskNinjaHub.MachineLearning.Api.Models;
using TaskNinjaHub.MachineLearning.Application;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Utilities.OperationResults;

namespace TaskNinjaHub.MachineLearning.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ForecastController(TrainingCore trainingCore) : ControllerBase
{
    private const string TrainedModelKeras = "trained_model.keras";

    [HttpPost("Train")]
    public OperationResult<string> Train([FromBody] List<CatalogTask> tasks)
    {
        var result = trainingCore.TrainAndSaveModel(tasks);
        return result;
    }

    [HttpPost("PredictProbability")]
    public OperationResult<double> PredictProbability([FromBody] TaskInputData inputData)
    {
        var probability = trainingCore.PredictProbability(inputData.PriorityId, inputData.InformationSystemId, inputData.TaskExecutorId, TrainedModelKeras);
        return probability;
    }
}