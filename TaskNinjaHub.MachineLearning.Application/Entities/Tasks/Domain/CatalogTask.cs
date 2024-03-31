using TaskNinjaHub.MachineLearning.Application.BaseUsers;
using TaskNinjaHub.MachineLearning.Application.Entities.Authors.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.InformationSystems.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.Priorities.Domain;
using TaskNinjaHub.MachineLearning.Application.Entities.TaskStatuses.Domain;
using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;
using File = TaskNinjaHub.MachineLearning.Application.Entities.Files.Domain.File;

namespace TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;

public class CatalogTask : BaseUserCU, IHaveId, IHaveName
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public int? TaskAuthorId { get; set; }

    public virtual Author? TaskAuthor { get; set; }

    public int? TaskExecutorId { get; set; }

    public virtual Author? TaskExecutor { get; set; }

    public int? InformationSystemId { get; set; }

    public virtual InformationSystem? InformationSystem { get; set; }

    public int? PriorityId { get; set; }

    public virtual Priority? Priority { get; set; }

    public int? TaskStatusId { get; set; }

    public virtual CatalogTaskStatus? TaskStatus { get; set; }

    public virtual List<File>? Files { get; set; }

    public int? OriginalTaskId { get; set; }

    public virtual CatalogTask? OriginalTask { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }
}