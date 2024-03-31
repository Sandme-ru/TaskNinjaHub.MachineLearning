using Newtonsoft.Json;
using Python.Runtime;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.TaskStatuses.Enum;

namespace TaskNinjaHub.MachineLearning.Application;

public class TrainingCore
{
    private const string ModelPy = "model.py";

    private const string TrainedModelKeras = "trained_model.keras";

    private const int Epochs = 100;

    public string TrainAndSaveModel(List<CatalogTask> tasks)
    {
        PythonEngine.Initialize();

        var projectDirectory = Directory.GetCurrentDirectory();
        var codeFilePath = Path.Combine(projectDirectory, ModelPy);

        if (File.Exists(codeFilePath))
        {
            var data = tasks.Select(task => new double[] {
                task.PriorityId ?? 0,
                task.InformationSystemId ?? 0,
                task.TaskExecutorId ?? 0
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

                ShutdownPythonEngine();

                return modelFilePath;
            }
        }
        else
        {
            ShutdownPythonEngine();

            return "File model.py not found in the project directory.";
        }
    }

    public double PredictProbability(double priorityId, double informationSystemId, double taskExecutorId, string modelFilePath)
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

                var jsonData = JsonConvert.SerializeObject(new { PriorityId = priorityId, InformationSystemId = informationSystemId, TaskExecutorId = taskExecutorId });
                result = (double)predictFunction(jsonData, modelFilePath).AsManagedObject(typeof(double));
            }

            ShutdownPythonEngine();

            return result;
        }
        else
        {
            ShutdownPythonEngine();

            throw new FileNotFoundException("File model.py not found in the project directory.");
        }
    }

    private static void ShutdownPythonEngine()
    {
        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", true);

        PythonEngine.Shutdown();

        AppContext.SetSwitch("System.Runtime.Serialization.EnableUnsafeBinaryFormatterSerialization", false);
    }
}