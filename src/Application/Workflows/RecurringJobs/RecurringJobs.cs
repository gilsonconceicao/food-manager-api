using Application.Workflows.Workflows;
using Domain.Extensions;
using Hangfire;

namespace Api.Workflows.RecurringJobs;

public static class RecurringJobsScheduler
{
    public static void Schedule()
    {

    }

    private static RecurringJobOptions AddRecurringJobOptions()
    {
        return new RecurringJobOptions
        {
            TimeZone = DateExtensions.GetBrazilTimeZone()
        };
    }
}
