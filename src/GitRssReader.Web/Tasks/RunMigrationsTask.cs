using GitRssReader.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace GitRssReader.Web.Tasks;

public class RunMigrationsTask : IHostedService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly ILogger _logger;

    public RunMigrationsTask(IDbContextFactory<AppDbContext> dbContextFactory, ILogger<RunMigrationsTask> logger)
    {
        _dbContextFactory = dbContextFactory;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Running migrations...");

        using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
