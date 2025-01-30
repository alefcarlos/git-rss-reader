using Fluxor;

namespace GitRssReader.Web.UnreadFeedUseCase;

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
