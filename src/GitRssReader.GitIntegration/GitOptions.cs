using System.ComponentModel.DataAnnotations;

namespace GitRssReader.GitIntegration;

public class GitOptions
{
    public const string Section = "Git";

    [Required]
    public required string Repository { get; set; }

    [Required]
    public required string Username { get; set; }

    [Required]
    public required string Password { get; set; }

    [Required]
    public required string OpmlFilePath { get; set; }
}
