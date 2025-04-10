using Api.Workflows.Workflows;
using Domain.Extensions;
using Hangfire;

namespace Api.Workflows.RecurringJobs;

public static class RecurringJobsScheduler
{
    private const string DailyAtMidnight = "00 20 * * *";

    public static void Schedule()
    {
        RecurringJob.AddOrUpdate<MergeUsersWorkflow>(
            recurringJobId: "merge-users-firebase-async",
            methodCall: process => process.MergeFirebaseUsersAsync(),
            cronExpression: DailyAtMidnight,
            new RecurringJobOptions
            {
                TimeZone = GenericExtenstions.GetBrazilTimeZone()
            }
        );
    }
}