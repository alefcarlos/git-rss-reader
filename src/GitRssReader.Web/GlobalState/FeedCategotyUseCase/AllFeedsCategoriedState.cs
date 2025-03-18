using Fluxor;

namespace GitRssReader.Web.GlobalState.FeedCategotyUseCase;

[FeatureState]
public class AllFeedsCategoriedState
{
    private readonly CategoriesCollection _feedCategories = [];

    public Dictionary<CategorySlug, FeedCollection> FeedCategories { get { return _feedCategories; } }

    private AllFeedsCategoriedState() { }
    public AllFeedsCategoriedState(CategoriesCollection data) { _feedCategories = data; }
}
