using Futurum.Microsoft.Extensions.DependencyInjection;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions;

public static partial class WebApplicationStartupExtensions
{
    /// <summary>
    /// Adds Core services for WebApiEndpoints
    /// </summary>
    public static IServiceCollection AddWebApiEndpointsCore(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddModule<FuturumWebApiCoreEndpointMicroModule>();

        return serviceCollection;
    }
}
