using Fluxor;
using GitRssReader.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace GitRssReader.Web.UnreadFeedUseCase;

public class FetchAllUnreadActionEffect : Effect<FetchAllUnreadAction>
{
    private readonly IDbContextFactory<AppDbContext> _dbFactory;

    public FetchAllUnreadActionEffect(IDbContextFactory<AppDbContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public override async Task HandleAsync(FetchAllUnreadAction action, IDispatcher dispatcher)
    {
        using var context = await _dbFactory.CreateDbContextAsync();

        var items = await context.GetTotalUnreadByFeedAsync();

        dispatcher.Dispatch(new FetchAllUnreadResultAction(items));
    }
}
