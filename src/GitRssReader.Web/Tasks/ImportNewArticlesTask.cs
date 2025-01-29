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
            using (var context = _dbFactory.CreateDbContext())
            {
                foreach (var category in feedsProvider.Data)
                {
                    foreach (var feed in category.Feeds)
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

                            foreach (var entry in entries.Where(x => x.PublishDate > control.LastImport))
                            {
                                context.Articles.Add(new Article
                                {
                                    Title = entry.Title,
                                    FeedSlug = feed.Slug,
                                    PublishedDate = entry.PublishDate.DateTime,
                                    Url = entry.Uri.ToString()
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Erro ao tentar processar o feed {feedTitle}", feed.Title);
                            break;
                        }

                        control.LastImport = DateTime.Now;
                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
            }

            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);
        }
    }
}
