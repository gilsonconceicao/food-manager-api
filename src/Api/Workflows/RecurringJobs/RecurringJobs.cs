using Api.Workflows.Workflows;
using Domain.Extensions;
using Hangfire;

namespace Api.Workflows.RecurringJobs;

public static class RecurringJobsScheduler
{
    public static void Schedule()
    {
        RecurringJob.AddOrUpdate<MergeUsersWorkflow>(
            recurringJobId: "merge-users-firebase-async",
            methodCall: process => process.MergeFirebaseUsersAsync(),
            cronExpression: "0 10 * * *",
            AddRecurringJobOptions()
        );

        RecurringJob.AddOrUpdate<PaymentStatusCheckWorkflow>(
            "check-pending-payments",
            job => job.CheckPendingPaymentsAsync(),
            "0 * * * *",
            AddRecurringJobOptions()
        );
    }

    private static RecurringJobOptions AddRecurringJobOptions()
    {
        return new RecurringJobOptions
        {
            TimeZone = GenericExtenstions.GetBrazilTimeZone()
        };
    }
}