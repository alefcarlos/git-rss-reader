
namespace GitRssReader.Web.Tasks;

public class InitializeFoldersTaks : IHostedService
{
    private readonly ILogger _logger;

    public InitializeFoldersTaks(ILogger<InitializeFoldersTaks> logger)
    {
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var tempFolder = Path.Combine(Path.GetTempPath(), "git-rss-reader-web");

        Directory.CreateDirectory(Path.Combine(tempFolder, "data"));

        var localRepoPath = Path.Combine(tempFolder, "repo");
        Directory.CreateDirectory(localRepoPath);

        _logger.LogInformation("Diretórios criados.");

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
