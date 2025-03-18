using GitRssReader.Web.Data;
using Microsoft.EntityFrameworkCore;
using SimpleFeedReader;

namespace GitRssReader.Web.Tasks;

public class ImportNewArticlesTask : BackgroundService
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger _logger;

    public ImportNewArticlesTask(IDbContextFactory<AppDbContext> dbFactory, IServiceProvider serviceProvider, ILogger<ImportNewArticlesTask> logger)
    {
        _dbFactory = dbFactory;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Yield();

        var feedsProvider = _serviceProvider.GetRequiredService<FeedsCollectionProvider>();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Iniciando scrapping de feeds..");

            await Task.WhenAll(feedsProvider.Data
                .Select(x => ProcessCategory(x.Key.Value, x.Value, stoppingToken))
                .ToArray());

            _logger.LogInformation("Scrapping de feeds finalizado");

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }

    public async Task ProcessCategory(string categorySlug, FeedCollection feeds, CancellationToken stoppingToken)
    {
        using var context = _dbFactory.CreateDbContext();

        foreach (var (key, feed) in feeds)
        {
            var control = await context.FeedsControl.FindAsync([feed.Slug], stoppingToken);
            if (control is null)
            {
                control = new FeedControl { FeedSlug = feed.Slug, LastImport = DateTime.MinValue };
                context.Add(control);
            }

            var reader = new FeedReader(true);
            try
            {
                var entries = reader.RetrieveFeed(feed.XmlUrl.ToString());

                //Verificar se tem novos artigos
                if (!entries.Any(x => x.PublishDate > control.LastImport))
                {
                    _logger.LogInformation("Não existe novos artigos para o feed {feedTitle}", feed.Title);
                    continue;
                }

                var tasks = entries
                        .Where(x => x.PublishDate > control.LastImport)
                        .Select(x => ProcessEntry(x, feed.Title, feed.Slug, categorySlug))
                        .ToArray();

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar processar o feed {feedTitle}", feed.Title);
                continue;
            }

            _logger.LogInformation("Novos artigos importados para o feed {feedTitle}", feed.Title);
            control.LastImport = DateTime.Now;
        }

        await context.SaveChangesAsync(stoppingToken);
    }

    public async Task ProcessEntry(FeedItem entry, string feedTitle, string feedSlug, string feedCategorySlug)
    {
        using var context = _dbFactory.CreateDbContext();

        context.Articles.Add(new Article
        {
            Title = entry.Title,
            FeedSlug = feedSlug,
            FeedName = feedTitle,
            FeedCategorySlug = feedCategorySlug,
            PublishedDate = entry.PublishDate.DateTime,
            Url = entry.Uri.ToString()
        });

        await context.SaveChangesAsync();
    }
}
