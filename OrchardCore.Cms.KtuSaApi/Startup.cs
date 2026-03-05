using FastEndpoints;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Cms.KtuSaApi.OpenApi;
using OrchardCore.Modules;
using Scalar.AspNetCore;

namespace OrchardCore.Cms.KtuSaApi;

public sealed class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddFastEndpoints(o => { o.Assemblies = [typeof(Startup).Assembly]; });
        services.AddOpenApi("ktu-sa-api",
            options =>
            {
                options.ShouldInclude = description => description.GroupName == "ktu-sa-api";
                options.AddDocumentTransformer<KtuSaOpenApiTransformer>();
            });
    }

    public override void Configure(IApplicationBuilder app, IEndpointRouteBuilder routes,
        IServiceProvider serviceProvider)
    {
        routes.MapFastEndpoints(c =>
        {
            c.Endpoints.Configurator = ep => { ep.Options(x => x.WithGroupName("ktu-sa-api")); };
            c.Errors.UseProblemDetails();
        });
        routes.MapOpenApi();
        routes.MapScalarApiReference();
    }
}