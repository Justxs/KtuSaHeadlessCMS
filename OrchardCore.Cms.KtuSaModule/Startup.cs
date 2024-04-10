using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Cms.KtuSaModule.Drivers.Fields;
using OrchardCore.Cms.KtuSaModule.Drivers.Parts;
using OrchardCore.Cms.KtuSaModule.Handlers;
using OrchardCore.Cms.KtuSaModule.Indexes;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Migrations;
using OrchardCore.Cms.KtuSaModule.Models.Fields;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Navigation;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;
using YesSql.Indexes;

namespace OrchardCore.Cms.KtuSaModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IGoogleCloudService, GoogleCloudService>();
        services.AddScoped<IStringActionService, StringActionService>();
        services.AddHttpClient<IFientaService, FientaService>();
        services.AddScoped<IRepository, Repository>();

        AddContentFields(services);
        AddContentParts(services);
        AddMigrations(services);
        AddPermissions(services);
        AddNavigationProviders(services);

        services.AddSingleton<IIndexProvider, MemberPartIndexProvider>();
        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();
    }

    public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
    {
    }

    private static void AddNavigationProviders(IServiceCollection services)
    {
        services.AddScoped<INavigationProvider, AdminMenu>();
        services.AddScoped<INavigationProvider, SponsorsMenu>();
        services.AddScoped<INavigationProvider, ArticlesMenu>();
        services.AddScoped<INavigationProvider, EventsMenu>();
        services.AddScoped<INavigationProvider, SaUnitsInfoMenu>();

    }

    private static void AddMigrations(IServiceCollection services)
    {
        services.AddScoped<IDataMigration, ArticleMigrations>();
        services.AddScoped<IDataMigration, DukMigrations>();
        services.AddScoped<IDataMigration, SponsorMigrations>();
        services.AddScoped<IDataMigration, ContactMigrations>();
        services.AddScoped<IDataMigration, HeroSectionMigrations>();
        services.AddScoped<IDataMigration, EventMigrations>();
        services.AddScoped<IDataMigration, SaUnitMigrations>();
        services.AddScoped<IDataMigration, UserMigrations>();
        services.AddScoped<IDataMigration, PositionMigrations>();

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

        services
            .AddContentField<QuillField>()
            .UseDisplayDriver<QuillFieldDriver>();
    }

    private static void AddPermissions(IServiceCollection services)
    {
        services.AddScoped<IPermissionProvider, DukPermissions>();
        services.AddScoped<IPermissionProvider, ArticlePermissions>();
        services.AddScoped<IPermissionProvider, EventPermissions>();
        services.AddScoped<IPermissionProvider, SponsorPermissions>();
        services.AddScoped<IPermissionProvider, HeroSectionPermissions>();
        services.AddScoped<IPermissionProvider, ContactPermissions>();
        services.AddScoped<IPermissionProvider, SaUnitPermissions>();

    }

    private static void AddContentParts(IServiceCollection services)
    {
        services.AddContentPart<ArticlePart>();
        services.AddContentPart<UserProfilePart>();

        services
            .AddContentPart<CardPart>()
            .UseDisplayDriver<CardPartDriver>()
            .AddHandler<CardPartHandler>();

        services
            .AddContentPart<SaUnitPart>()
            .UseDisplayDriver<SaUnitPartDriver>();

        services
            .AddContentPart<EventPart>()
            .UseDisplayDriver<EventPartDriver>()
            .AddHandler<EventPartHandler>();

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
            .AddHandler<PositionPartHandler>();

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