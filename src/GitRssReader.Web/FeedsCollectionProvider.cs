using OPMLCore.NET;
using GitRssReader.GitIntegration;
using System.Xml.Linq;

namespace GitRssReader.Web;
public record FeedInfo(string Title, string Slug, Uri XmlUrl, Uri SiteUrl, string Description);

public partial class FeedsCollectionProvider
{
    public Dictionary<CategorySlug, string> CategoriesTitles { get; set; } = [];
    public Dictionary<FeedSlug, string> FeedsTitles { get; set; } = [];

    public CategoriesCollection Data { get; set; } = [];

    public FeedsCollectionProvider()
    {
        var source = new Opml(OpmlFileProvider.Instance.FilePath);

        foreach (var category in source.Body.Outlines)
        {
            var slug = SlugGenerator.GenerateSlug(category.Text);
            var categorySlug = new CategorySlug(slug);
            CategoriesTitles.Add(categorySlug, category.Text);

            Data.Add(categorySlug, []);

            foreach (var feed in category.Outlines)
            {
                var entry = new FeedInfo(feed.Text, SlugGenerator.GenerateSlug(feed.Text), new Uri(feed.XMLUrl), new Uri(feed.HTMLUrl), feed.Description);
                var entrySlug = new FeedSlug(entry.Slug);
                FeedsTitles.TryAdd(entrySlug, feed.Text);

                if (!Data[categorySlug].TryGetValue(entrySlug, out _))
                {
                    Data[categorySlug].Add(entrySlug, entry);
                }
            }
        }
    }

    public void AddFeed(NewFeedInfo feed, string category)
    {
        FeedsTitles.Add(new(feed.Title), feed.Title);

        var doc = XDocument.Load(OpmlFileProvider.Instance.FilePath!);

        var body = doc.Root!.Element("body")!;

        var targetOutline = body.Elements("outline")
            .FirstOrDefault(o => (string)o.Attribute("text")! == CategoriesTitles[new(category)]);

        var newOutline = new XElement("outline",
            new XAttribute("text", feed.Title),
            new XAttribute("type", "rss"),
            new XAttribute("xmlUrl", feed.XmlUrl),
            new XAttribute("htmlUrl", feed.HtmlUrl),
            new XAttribute("description", feed.Description)
        );

        targetOutline!.Add(newOutline);

        var targetCategory = Data[new CategorySlug(category)];
        targetCategory.Add(new FeedSlug(SlugGenerator.GenerateSlug(feed.Title)), new FeedInfo(feed.Title, SlugGenerator.GenerateSlug(feed.Title), feed.XmlUrl, feed.HtmlUrl, feed.Description));

        doc.Save(OpmlFileProvider.Instance.FilePath!);
    }
}

public record NewFeedInfo(string Title, Uri XmlUrl, Uri HtmlUrl, string Description);
