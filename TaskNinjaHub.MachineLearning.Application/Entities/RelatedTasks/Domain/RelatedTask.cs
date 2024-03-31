using TaskNinjaHub.MachineLearning.Application.BaseUsers;
using TaskNinjaHub.MachineLearning.Application.Entities.Tasks.Domain;
using TaskNinjaHub.MachineLearning.Application.Interfaces.Haves;

namespace TaskNinjaHub.MachineLearning.Application.Entities.RelatedTasks.Domain;

public class RelatedTask : BaseUserCU, IHaveId
{
    public int Id { get; set; }

    public int MainTaskId { get; set; }

    public virtual CatalogTask? MainTask { get; set; }

    public int SubordinateTaskId { get; set; }

    public virtual CatalogTask? SubordinateTask { get; set; }
}