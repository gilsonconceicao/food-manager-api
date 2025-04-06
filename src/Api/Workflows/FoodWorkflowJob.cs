using Domain.Interfaces.Workflow;
using Hangfire;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Api.Workflows.FoodWorkflowJob;

public class FoodWorkflowJob : ICartWorkflowJob
{
    private readonly DataBaseContext _context;
    private readonly IBackgroundJobClient _backgroundJobClient;
    private readonly IRecurringJobManager _recurringJobManager;
    private readonly ILogger<FoodWorkflowJob> _logger;

    public FoodWorkflowJob(IBackgroundJobClient backgroundJobClient, IRecurringJobManager recurringJobManager, ILogger<FoodWorkflowJob> logger, DataBaseContext context)
    {
        _context = context;
        _backgroundJobClient = backgroundJobClient;
        _recurringJobManager = recurringJobManager;
        _logger = logger;
    }

    [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    public async Task CheckCartQuantityAsync(Guid cartId, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation($"[Hangfire Job] Checking quality of food {cartId}...");
        Task.Delay(500); 
        _logger.LogInformation($"✅ Quality check completed for food {cartId}!");
    }
}