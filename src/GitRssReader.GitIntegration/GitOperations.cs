using LibGit2Sharp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitRssReader.GitIntegration;

public class GitOperations
{
    private readonly GitOptions _options;
    private readonly ILogger _logger;

    public GitOperations(IOptions<GitOptions> options, ILogger<GitOperations> logger)
    {
        _options = options.Value;
        _logger = logger;
    }

    public void Clone()
    {
        var localRepoPath = Path.Combine(Path.GetTempPath(), "git-rss-reader-web", "repo");

        if (!Directory.Exists(localRepoPath))
        {
            Directory.CreateDirectory(localRepoPath);
        }

        var opmlFile = Path.Combine(localRepoPath, _options.OpmlFilePath);

        OpmlFileProvider.Instance.SetRepoPath(localRepoPath);

        if (File.Exists(opmlFile))
        {
            _logger.LogInformation("The repo is already cloned, nothing to do.");
            OpmlFileProvider.Instance.SetFilePath(opmlFile);
            return;
        }

        _logger.LogInformation("Cloning repository into {localPath}...", localRepoPath);

        var options = new CloneOptions
        {
            FetchOptions =
                {
                    CredentialsProvider = (url, user, cred) => new UsernamePasswordCredentials
                    {
                        Username = _options.Username,
                        Password = _options.Password
                    }
                }
        };

        Repository.Clone(_options.Repository, localRepoPath, options);
        OpmlFileProvider.Instance.SetFilePath(opmlFile);

        _logger.LogInformation("Repository cloned.");
    }
}
