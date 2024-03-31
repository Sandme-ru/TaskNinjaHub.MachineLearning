using System.Text.Json.Serialization;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;

namespace TaskNinjaHub.MachineLearning.Application.Entities.Files.Domain;

public class File : IHaveId, IHaveName, IHaveDateCreated
{
    public int Id { get; set; }
    
    public string? Name { get; set; }

    public DateTime? DateCreated { get; set; }
    
    public int? TaskId { get; set; }

    [JsonIgnore]
    public virtual CatalogTask? Task { get; set; }
}