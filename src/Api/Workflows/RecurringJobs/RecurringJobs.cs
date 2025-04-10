using Api.Workflows.Workflows;
using Hangfire;

namespace Api.Workflows.RecurringJobs;
public static class RecurringJobsScheduler
{
    private const string DailyAtMidnight = "0 0 * * *";

    public static void Schedule()
    {
        RecurringJob.AddOrUpdate<MergeUsersWorkflow>(
            recurringJobId: "merge-users-firebase-async",
            methodCall: process => process.MergeFirebaseUsersAsync(),
            cronExpression: DailyAtMidnight,
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
            }
        ); 
    }
}