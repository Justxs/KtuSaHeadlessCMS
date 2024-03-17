using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Cms.KtuSaModule.Drivers;
using OrchardCore.Cms.KtuSaModule.Handlers;
using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Migrations;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using YesSql.Indexes;

namespace OrchardCore.Cms.KtuSaModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGoogleCloudService, GoogleCloudService>();

        AddContentFields(services);
        AddContentParts(services);
        AddMigrations(services);

        services.AddSingleton<IIndexProvider, MemberPartIndexProvider>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
    }

    private static void AddMigrations(IServiceCollection services)
    {
        services.AddScoped<IDataMigration, ArticleMigrations>();
        services.AddScoped<IDataMigration, DukMigrations>();
        services.AddScoped<IDataMigration, SponsorMigrations>();
        services.AddScoped<IDataMigration, ContactMigrations>();
        services.AddScoped<IDataMigration, HeroSectionMigrations>();
    }

    private static void AddContentFields(IServiceCollection services)
    {
        services
            .AddContentField<ImageUploadField>()
            .UseDisplayDriver<ImageUploadFieldDriver>()
            .AddHandler<ImageUploadFieldHandler>();

        services
            .AddContentField<SaUnitSelectField>()
            .UseDisplayDriver<SaUnitSelectFieldDriver>();
    }

    private static void AddContentParts(IServiceCollection services)
    {
        services
            .AddContentPart<ArticlePart>()
            .UseDisplayDriver<ArticlePartDriver>()
            .AddHandler<ArticlePartHandler>();

        services
            .AddContentPart<ContactPart>()
            .UseDisplayDriver<ContactPartDriver>();

        services
            .AddContentPart<MemberPart>()
            .UseDisplayDriver<MemberPartDriver>()
            .AddHandler<MemberPartHandler>();

        services
            .AddContentPart<AddressPart>()
            .UseDisplayDriver<AddressPartDriver>();

        services
            .AddContentPart<PositionPart>()
            .UseDisplayDriver<PositionPartDriver>()
            .AddHandler<MemberPartHandler>();

        services
            .AddContentPart<DukPart>()
            .UseDisplayDriver<DukPartDriver>()
            .AddHandler<DukPartHandler>();

        services
            .AddContentPart<SponsorPart>()
            .UseDisplayDriver<SponsorPartDriver>()
            .AddHandler<SponsorPartHandler>();

        services.AddContentPart<HeroSectionPart>();
    }
}