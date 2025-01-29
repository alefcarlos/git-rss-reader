using GitRssReader.GitIntegration;

namespace GitRssReader.Web.GitTasks;

public class CloneRepositoryTask : IHostedService
{
    private readonly GitOperations _gitOperations;

    public CloneRepositoryTask(GitOperations gitOperations)
    {
        _gitOperations = gitOperations;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _gitOperations.Clone();

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
