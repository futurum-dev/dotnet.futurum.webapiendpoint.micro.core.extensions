using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro.Core.Extensions.Validation;

using Microsoft.Extensions.DependencyInjection;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions;

public class FuturumWebApiCoreEndpointMicroModule : IModule
{
    public void Load(IServiceCollection services)
    {
        services.AddSingleton(typeof(IFluentValidationService<>), typeof(FluentValidationService<>));
        services.AddSingleton<IDataAnnotationsValidationService, DataAnnotationsValidationService>();
        services.AddSingleton(typeof(IValidationService<>), typeof(ValidationService<>));
    }
}
