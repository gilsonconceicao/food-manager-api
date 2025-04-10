using Api.Workflows.Workflows;
using Hangfire;

namespace Api.Workflows.RecurringJobs;
public static class RecurringJobsScheduler
{
    public static void Schedule()
    {
        RecurringJob.AddOrUpdate<IUserActivity>(
            recurringJobId: "merge-users-firebase-async",
            methodCall: servico => servico.ProcessMergeUsersFirebaseAsyc(),
            cronExpression: "0 0 * * *",
            new RecurringJobOptions
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time")
            }
        ); 
    }
}