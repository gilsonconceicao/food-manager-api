using Application.Workflows.Workflows;
using Domain.Extensions;
using Hangfire;

namespace Api.Workflows.RecurringJobs;

public static class RecurringJobsScheduler
{
    public static void Schedule()
    {
        // RecurringJob.AddOrUpdate<MergeUsersWorkflow>(
        //     recurringJobId: "merge-users-firebase-async",
        //     methodCall: process => process.MergeFirebaseUsersAsync(),
        //     cronExpression: "0 10 * * *",
        //     AddRecurringJobOptions()
        // );

        // RecurringJob.AddOrUpdate<TryHangFireWorkflow>(
        //     recurringJobId: "try-logger-email",
        //     methodCall: process => process.TryAsync(),
        //     cronExpression: "0 18 * * *",
        //     AddRecurringJobOptions()
        // );
    }

    private static RecurringJobOptions AddRecurringJobOptions()
    {
        return new RecurringJobOptions
        {
            TimeZone = DateExtensions.GetBrazilTimeZone()
        };
    }
}
