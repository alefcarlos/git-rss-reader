using System.Text.RegularExpressions;
using System.Text;

namespace GitRssReader.Web;

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
