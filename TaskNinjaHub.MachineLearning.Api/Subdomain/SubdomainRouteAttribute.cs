﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace TaskNinjaHub.MachineLearning.Api.Subdomain;

public class SubdomainRouteAttribute : IDocumentFilter
{
    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        foreach (var path in swaggerDoc.Paths.ToList())
        {
            swaggerDoc.Paths.Remove(path.Key);
            var newPathKey = "/machine-learning" + path.Key;
            swaggerDoc.Paths.Add(newPathKey, path.Value);
        }
    }
}