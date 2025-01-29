namespace GitRssReader.GitIntegration;

public sealed class OpmlFileProvider
{
    private static readonly OpmlFileProvider _instance = new();

    public string? FilePath { get; private set; }
    public string? RepoPath { get; private set; }

    private OpmlFileProvider()
    {
    }

    public static OpmlFileProvider Instance { get { return _instance; } }

    internal void SetFilePath(string path)
    {
        FilePath = path;
    }

    internal void SetRepoPath(string path)
    {
        RepoPath = path;
    }
}
