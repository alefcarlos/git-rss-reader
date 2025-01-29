using LibGit2Sharp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GitRssReader.GitIntegration;

public class GitOperations
{
    private readonly GitOptions _options;
    private readonly ILogger _logger;
    private readonly IConfiguration _configuration;

    public GitOperations(IOptions<GitOptions> options, ILogger<GitOperations> logger, IConfiguration configuration)
    {
        _options = options.Value;
        _logger = logger;
        _configuration = configuration;
    }

    public void Clone()
    {
        try
        {
            var localRepoPath = Path.Combine(Path.GetTempPath(), "repos", Guid.NewGuid().ToString());

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

            OpmlFileProvider.Instance.SetFilePath(Path.Combine(localRepoPath, _options.OpmlFilePath));

            _logger.LogInformation("Repository cloned.");
        }
        catch (LibGit2SharpException)
        {
            throw;
        }
    }
}
