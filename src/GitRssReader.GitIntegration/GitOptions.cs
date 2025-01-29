namespace GitRssReader.GitIntegration;

public class GitOptions
{
    public const string Section = "Git";
    public required string Repository { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string OpmlFilePath { get; set; }
}
