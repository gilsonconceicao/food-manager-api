using Api.Workflows.JobSchedulerService;
using Hangfire;

namespace Api.Workflows.Workflows;

interface IUserWorkflow
{
    Task CreateUserJobAsync();
}

public class UserWorkflow : IUserWorkflow
{
    private readonly IJobSchedulerService _jobScheduler;

    public UserWorkflow(IJobSchedulerService jobScheduler)
    {
        _jobScheduler = jobScheduler;
    }

    public Task CreateUserJobAsync()
    {
        _jobScheduler.Schedule<UserActivity>(
            activity => activity.ProcessCreateUser(),
            TimeSpan.FromMinutes(5)
        );

        return Task.CompletedTask;
    }
}