using Newtonsoft.Json;
using Python.Runtime;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.TaskStatuses.Enum;

namespace TaskNinjaHub.MachineLearning.Application;

public class Core
{
    public dynamic Main()
    {
        PythonEngine.Initialize();
        
        var projectDirectory = Directory.GetCurrentDirectory();

        var codeFilePath = Path.Combine(projectDirectory, "code.py");

        if (File.Exists(codeFilePath))
        {
            var code = File.ReadAllText(codeFilePath);

            using (Py.GIL())
            {
                using dynamic scope = Py.CreateScope();

                scope.Exec(code);

                var mainFunction = scope.main;
                var sum = mainFunction("firstInt, secondInt");
                return JsonConvert.SerializeObject(sum.ToString());
            }
        }
        else
        {
            return "File Code.py not found in the project directory.";
        }
    }

    public string TrainAndSaveModel(List<CatalogTask> tasks)
    {
        PythonEngine.Initialize();

        var projectDirectory = Directory.GetCurrentDirectory();

        var codeFilePath = Path.Combine(projectDirectory, "model.py");

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
                var model = trainModel(data, labels, 100);
                var modelFilePath = Path.Combine(projectDirectory, "trained_model.keras");
                saveModel(model, modelFilePath);
                return modelFilePath;
            }
        }
        else
        {
            return "File model.py not found in the project directory.";
        }
    }

    public double PredictProbability(double priorityId, double informationSystemId, double taskExecutorId, string modelFilePath)
    {
        PythonEngine.Initialize();

        var projectDirectory = Directory.GetCurrentDirectory();

        var codeFilePath = Path.Combine(projectDirectory, "model.py");

        if (File.Exists(codeFilePath))
        {
            using (Py.GIL())
            {
                dynamic scope = Py.CreateScope();
                scope.Exec(File.ReadAllText(codeFilePath));

                var predictFunction = scope.predict_probability;

                var jsonData = JsonConvert.SerializeObject(new { PriorityId = priorityId, InformationSystemId = informationSystemId, TaskExecutorId = taskExecutorId });
                return predictFunction(jsonData, modelFilePath);
            }
        }
        else
        {
            throw new FileNotFoundException("File model.py not found in the project directory.");
        }
    }
}