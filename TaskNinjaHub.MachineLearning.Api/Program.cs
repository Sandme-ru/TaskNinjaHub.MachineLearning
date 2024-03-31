using Python.Runtime;
using TaskNinjaHub.MachineLearning.Application;

var builder = WebApplication.CreateBuilder(args);

Runtime.PythonDLL = @"C:\Users\Zaid.Mingaliev\AppData\Local\anaconda3\envs\myenv\python312.dll";

builder.Services.AddScoped<Core>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();