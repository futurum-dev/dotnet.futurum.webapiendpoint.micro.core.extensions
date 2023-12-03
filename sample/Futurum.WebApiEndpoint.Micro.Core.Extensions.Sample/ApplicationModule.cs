using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Blog;
using Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample.Todo;

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample;

public class ApplicationModule : IModule
{
    private readonly IConfiguration _configuration;

    public ApplicationModule(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Load(IServiceCollection services)
    {
        services.AddModule<BlogModule>();
        services.AddModule(new TodoModule(_configuration));
    }
}