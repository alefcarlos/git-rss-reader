using Fluxor;

namespace GitRssReader.Web.Feed;

[FeatureState]
public class UnreadFeedsState
{
    private Dictionary<string, int> _unreadFeeds { get; } = [];

    public IReadOnlyDictionary<string, int> UnreadFeeds { get { return _unreadFeeds.AsReadOnly(); } }

    private UnreadFeedsState() { }

    public UnreadFeedsState(Dictionary<string, int> unreadFeeds)
    {
        _unreadFeeds = unreadFeeds;
    }
}
