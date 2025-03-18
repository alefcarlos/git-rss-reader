using GitRssReader.Web;

namespace Microsoft.Extensions.DependencyInjection;

public static class FeedsCollectionServiceCollectionExtensions
{
    public static IServiceCollection AddFeedsCollectionProvider(this IServiceCollection services)
    {
        services.AddActivatedSingleton<FeedsCollectionProvider>();

        return services;
    }
}
