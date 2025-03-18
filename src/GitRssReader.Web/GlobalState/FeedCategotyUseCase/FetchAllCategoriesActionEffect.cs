using Fluxor;
using GitRssReader.Web.UnreadFeedUseCase;

namespace GitRssReader.Web.GlobalState.FeedCategotyUseCase;

public class FetchAllCategoriesActionEffect : Effect<FetchAllCategoriesAction>
{
    private readonly FeedsCollectionProvider _feedsCollectionProvider;

    public FetchAllCategoriesActionEffect(FeedsCollectionProvider feedsCollectionProvider)
    {
        _feedsCollectionProvider = feedsCollectionProvider;
    }

    public override Task HandleAsync(FetchAllCategoriesAction action, IDispatcher dispatcher)
    {
        dispatcher.Dispatch(new FetchAllCategoriesResultAction(_feedsCollectionProvider.Data));
        return Task.CompletedTask;
    }
}
