using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;

namespace TaskNinjaHub.MachineLearning.Application.Entities.Priorities.Domain;

public class Priority : IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}