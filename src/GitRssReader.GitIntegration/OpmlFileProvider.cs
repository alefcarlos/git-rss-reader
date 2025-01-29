namespace GitRssReader.GitIntegration;

public sealed class OpmlFileProvider
{
    private static OpmlFileProvider? _instance;

    public string? FilePath { get; private set; }

    private OpmlFileProvider()
    {
    }

    public static OpmlFileProvider Instance { get { return _instance ??= new(); } }


    internal void SetFilePath(string path)
    {
        FilePath = path;
    }
}
