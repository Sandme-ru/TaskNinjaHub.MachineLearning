using TaskNinjaHub.MachineLearning.Application.BaseUsers;
using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;

namespace TaskNinjaHub.MachineLearning.Application.Entities.TaskStatuses.Domain;

public class CatalogTaskStatus: BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }
}