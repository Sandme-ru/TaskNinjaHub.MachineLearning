using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TaskNinjaHub.MachineLearning.Application.BaseUsers;
using TaskNinjaHub.MachineLearning.Application.Entities.Authors.Enums;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;

namespace TaskNinjaHub.MachineLearning.Application.Entities.Authors.Domain;

public class Author: BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? ShortName { get; set; }

    public string? RoleName { get; set; }

    public string? AuthGuid { get; set; }

    [JsonIgnore]
    public virtual List<CatalogTask>? ExecutableTasks { get; set; }

    [JsonIgnore]
    public virtual List<CatalogTask>? AssignedTasks { get; set; }

    public LocalizationType? LocalizationType { get; set; }

    [NotMapped]
    public int? CountPerformedTasks { get; set; }

    [NotMapped] 
    public string? FullName => $"{ShortName} [{CountPerformedTasks}]";
}