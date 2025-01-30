using Fluxor;
using GitRssReader.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace GitRssReader.Web.Feed;

public class MarkFeedAsReadAction
{
    public required string Feed { get; set; }
}

public record FetchAllUnreadAction;
public record FetchAllUnreadResultAction(Dictionary<string, int> Data);

public static class Reducers
{
    [ReducerMethod]
    public static UnreadFeedsState ReduceMarkFeedAsReadAction(
        UnreadFeedsState state, MarkFeedAsReadAction action)
    {
        var copy = new Dictionary<string, int>();
        foreach (var (key, value) in state.UnreadFeeds)
        {
            if (!string.Equals(key, action.Feed))
            {
                copy.Add(key, value);
            }
        }

        return new UnreadFeedsState(copy);
    }


    [ReducerMethod]
    public static UnreadFeedsState ReduceFetchDataResultAction(UnreadFeedsState state, FetchAllUnreadResultAction action)
    {
        return new UnreadFeedsState(action.Data);
    }
}

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
