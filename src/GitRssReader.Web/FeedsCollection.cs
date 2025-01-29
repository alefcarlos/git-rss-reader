using System.Text.RegularExpressions;
using System.Text;
using OPMLCore.NET;

namespace Microsoft.Extensions.DependencyInjection;

public record FeedCategory(string Title, string Slug, IEnumerable<FeedEntry> Feeds);
public record FeedEntry(string Title, string Slug, Uri XmlUrl, Uri SiteUrl, string Description);

public static class SlugGenerator
{
    public static string GenerateSlug(string input, bool useUnderscore = true)
    {
        // Passo 1: Normalizar a string (remover acentos)
        string normalized = input.Normalize(NormalizationForm.FormD);
        Regex regex = new Regex(@"\p{IsCombiningDiacriticalMarks}+");
        string withoutDiacritics = regex.Replace(normalized, string.Empty);

        // Passo 2: Remover caracteres especiais (exceto letras, números e espaços)
        string cleaned = Regex.Replace(withoutDiacritics, @"[^a-zA-Z0-9\s-]", "");

        // Passo 3: Substituir espaços por hífens/underscores e converter para minúsculas
        string separator = useUnderscore ? "_" : "-";
        string slug = Regex.Replace(cleaned, @"\s+", separator).ToLower();

        // Passo 4: Remover hífens/underscores repetidos e dos extremos
        slug = Regex.Replace(slug, $"{separator}+", separator);
        slug = slug.Trim(separator.ToCharArray());

        return slug;
    }
}

public class FeedsCollectionProvider
{
    public FeedsCollectionProvider()
    {
        var opml = new Opml("./opml.xml");

        foreach (var category in opml.Body.Outlines)
        {
            var feeds = category.Outlines.Select(i => new FeedEntry(i.Text, SlugGenerator.GenerateSlug(i.Text), new Uri(i.XMLUrl), new Uri(i.HTMLUrl), i.Description));
            var categoryEntry = new FeedCategory(category.Text, SlugGenerator.GenerateSlug(category.Text), feeds);

            Data.Add(categoryEntry);
        }
    }

    public List<FeedCategory> Data { get; set; } = [];
}

public static class FeedsCollectionServiceCollectionExtensions
{
    public static IServiceCollection AddFeedsCollectionProvider(this IServiceCollection services)
    {
        services.AddActivatedSingleton<FeedsCollectionProvider>();

        return services;
    }
}
