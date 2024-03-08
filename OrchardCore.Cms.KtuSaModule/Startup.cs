using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Cms.KtuSaModule.Drivers;
using OrchardCore.Cms.KtuSaModule.Handlers;
using OrchardCore.Cms.KtuSaModule.Migrations;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;

namespace OrchardCore.Cms.KtuSaModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGoogleDriveService, GoogleDriveService>();

        services
            .AddContentPart<ArticlePart>()
            .UseDisplayDriver<ArticlePartDriver>()
            .AddHandler<ArticlePartHandler>();

        services.AddScoped<IDataMigration, ArticleMigrations>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
    }
}