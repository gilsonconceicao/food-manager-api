using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(IgnoreApi = true)] 
public class HangfireJobsController : ControllerBase
{
    private readonly IWebHostEnvironment _env;

    public HangfireJobsController(IWebHostEnvironment env)
    {
        _env = env;
    }

    private IActionResult DevOnly()
    {
        return Forbid("Este endpoint só está disponível em ambiente de desenvolvimento.");
    }

    [HttpDelete("cancel/{jobId}")]
    public IActionResult CancelRecurringJob(string jobId)
    {
        if (!_env.IsDevelopment()) return DevOnly();

        RecurringJob.RemoveIfExists(jobId);
        return Ok($"Job recorrente '{jobId}' cancelado com sucesso.");
    }


    [HttpDelete("cancel-all")]
    public IActionResult CancelAllRecurringJobs()
    {
        if (!_env.IsDevelopment()) return DevOnly();

        var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();

        foreach (var job in recurringJobs)
        {
            RecurringJob.RemoveIfExists(job.Id);
        }

        return Ok("Todos os jobs recorrentes foram cancelados com sucesso.");
    }
}
