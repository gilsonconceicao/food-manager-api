using Domain.Interfaces.Workflow;
using Hangfire;

namespace Api.Workflows.RecurringJobs;
public static class RecurringJobsScheduler
{
    public static void Schedule()
    {
        // RecurringJob.AddOrUpdate<ICartWorkflowJob>(
        //     "fix-cart-quantities-every-5-mins",
        //     job => job.ValidationQuantityJob(),
        //     "*/5 * * * *"
        // );
    }
}