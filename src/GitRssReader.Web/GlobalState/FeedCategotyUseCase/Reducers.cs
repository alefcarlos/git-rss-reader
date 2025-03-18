using Fluxor;
using GitRssReader.Web.GlobalState.FeedCategotyUseCase;

namespace GitRssReader.Web.FeedCategotyUseCase;

public static class Reducers
{
    [ReducerMethod]
    public static AllFeedsCategoriedState ReduceFetchDataResultAction(AllFeedsCategoriedState state, FetchAllCategoriesResultAction action)
    {
        return new AllFeedsCategoriedState(action.Data);
    }
}
