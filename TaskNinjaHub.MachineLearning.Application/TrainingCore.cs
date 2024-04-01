using Newtonsoft.Json;
using Python.Runtime;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.TaskStatuses.Enum;
using TaskNinjaHub.MachineLearning.Application.Utilities.OperationResults;

namespace TaskNinjaHub.MachineLearning.Application;

public class TrainingCore
{
    private const string ModelPy = "model.py";

    private const string TrainedModelKeras = "trained_model.keras";

    private const int Epochs = 100;

    public OperationResult<string> TrainAndSaveModel(List<CatalogTask> tasks)
    {
        try
        {
            PythonEngine.Initialize();

            var projectDirectory = Directory.GetCurrentDirectory();
            var codeFilePath = Path.Combine(projectDirectory, ModelPy);

            if (File.Exists(codeFilePath))
            {
                var data = tasks.Select(task => new double[]
                {
                    task.PriorityId ?? 0,
                    task.InformationSystemId ?? 0,
                    task.TaskTypeId ?? 0,
                    task.TaskExecutorId ?? 0,

                }).ToArray();

                var labels = tasks.Select(task => task.TaskStatusId == (int?)EnumTaskStatus.Done ? 1 : 0).ToArray();

                dynamic scope;
                using (Py.GIL())
                {
                    scope = Py.CreateScope();
                    scope.Exec(File.ReadAllText(codeFilePath));
                }

                var trainModel = scope.train_model;
                var saveModel = scope.save_model;

                using (Py.GIL())
                {
                    var model = trainModel(data, labels, Epochs);
                    var modelFilePath = Path.Combine(projectDirectory, TrainedModelKeras);
                    saveModel(model, modelFilePath);
                }

                ShutdownPythonEngine();

                return OperationResult<string>.SuccessResult();
            }
            else
            {
                ShutdownPythonEngine();

                return OperationResult<string>.FailedResult("File model.py not found in the project directory.");
            }
        }
        catch (Exception e)
        {
            ShutdownPythonEngine();

            return OperationResult<string>.FailedResult(e.Message);
        }
    }

    public OperationResult<double> PredictProbability(double priorityId, double informationSystemId, double taskTypeId, double taskExecutorId, string modelFilePath)
    {
        try
        {
            PythonEngine.Initialize();

            var projectDirectory = Directory.GetCurrentDirectory();

            var codeFilePath = Path.Combine(projectDirectory, ModelPy);

            if (File.Exists(codeFilePath))
            {
                dynamic result;
                using (Py.GIL())
                {
                    dynamic scope = Py.CreateScope();
                    scope.Exec(File.ReadAllText(codeFilePath));

                    var predictFunction = scope.predict_probability;

                    var jsonData = JsonConvert.SerializeObject(new
                    {
                        PriorityId = priorityId, 
                        InformationSystemId = informationSystemId,
                        TaskTypeId = taskTypeId,
                        TaskExecutorId = taskExecutorId
                    });
                    result = (double)predictFunction(jsonData, modelFilePath).AsManagedObject(typeof(double));
                }

                ShutdownPythonEngine();

                return OperationResult<double>.SuccessResult(result);
            }
            else
            {
                ShutdownPythonEngine();

                return OperationResult<double>.FailedResult("File model.py not found in the project directory.");
            }
        }
        catch (Exception e)
        {
            ShutdownPythonEngine();

            return OperationResult<double>.FailedResult(e.Message);
        }
    }

    private static void ShutdownPythonEngine()
    {
        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);

        PythonEngine.Shutdown();

        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false);
    }
}