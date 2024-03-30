using Newtonsoft.Json;
using Python.Runtime;

namespace TaskNinjaHub.MachineLearning.Application;

public class Core
{
    public dynamic Main()
    {
        Runtime.PythonDLL = @"C:\Users\Zaid.Mingaliev\AppData\Local\anaconda3\envs\myenv\python312.dll";

        PythonEngine.Initialize();
        
        var projectDirectory = Directory.GetCurrentDirectory();

        var codeFilePath = Path.Combine(projectDirectory, "Code.py");

        if (File.Exists(codeFilePath))
        {
            var code = File.ReadAllText(codeFilePath);

            using (Py.GIL())
            {
                using dynamic scope = Py.CreateScope();

                scope.Exec(code);

                var mainFunction = scope.main;
                var sum = mainFunction("firstInt, secondInt");
                return JsonConvert.SerializeObject(sum);
            }
        }
        else
        {
            return "File Code.py not found in the project directory.";
        }
    }
}