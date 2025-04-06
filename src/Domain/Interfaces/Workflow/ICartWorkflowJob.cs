namespace Domain.Interfaces.Workflow;
public interface ICartWorkflowJob
{
    Task CheckCartQuantityAsync(Guid cartId, CancellationToken cancellationToken = default);
}
