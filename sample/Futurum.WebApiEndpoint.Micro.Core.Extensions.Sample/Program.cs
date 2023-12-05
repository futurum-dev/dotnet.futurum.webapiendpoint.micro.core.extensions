using Futurum.Microsoft.Extensions.DependencyInjection;
using Futurum.WebApiEndpoint.Micro;
using Futurum.WebApiEndpoint.Micro.Core.Extensions;
using Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddProblemDetails();

builder.Services.AddModule(new ApplicationModule(builder.Configuration));

builder.Services
       .AddWebApiEndpoints(new WebApiEndpointConfiguration(new WebApiEndpointVersion(1, 0))
       {
           DefaultOpenApiInfo = new OpenApiInfo
           {
               Title = "Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample",
           },
           OpenApiDocumentVersions =
           {
               {
                   new WebApiEndpointVersion(3, 0),
                   new OpenApiInfo
                   {
                       Title = "Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample v3"
                   }
               }
           }
       })
       .AddWebApiEndpointsCore()
       .AddWebApiEndpointsForFuturumWebApiEndpointMicroCoreExtensionsSample();

var app = builder.Build();

app.UseWebApiEndpoints();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
    app.MapGet("/error", () => Results.Problem("An error occurred.", statusCode: 500))
       .ExcludeFromDescription();

    app.UseWebApiEndpointsOpenApi();
}

app.Run();

namespace Futurum.WebApiEndpoint.Micro.Core.Extensions.Sample
{
    public partial class Program
    {
    }
}
