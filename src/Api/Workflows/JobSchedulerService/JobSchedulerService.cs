using System.Linq.Expressions;
using Hangfire;

namespace Api.Workflows.JobSchedulerService;

public interface IJobSchedulerService
{
    void Enqueue<T>(Expression<Func<T, Task>> methodCall);
    void Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
    void Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt);
    void AddOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression);
    void RemoveRecurringJob(string jobId);
}

public class JobSchedulerService : IJobSchedulerService
{
    public void Enqueue<T>(Expression<Func<T, Task>> methodCall)
    {
        BackgroundJob.Enqueue(methodCall);
    }

    public void Schedule<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
    {
        BackgroundJob.Schedule(methodCall, delay);
    }

    public void Schedule<T>(Expression<Func<T, Task>> methodCall, DateTimeOffset enqueueAt)
    {
        BackgroundJob.Schedule(methodCall, enqueueAt);
    }

    public void AddOrUpdateRecurring<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression)
    {
        RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);
    }

    public void RemoveRecurringJob(string jobId)
    {
        RecurringJob.RemoveIfExists(jobId);
    }
}