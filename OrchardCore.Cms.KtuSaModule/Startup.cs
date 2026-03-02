using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OrchardCore.Cms.KtuSaModule.Drivers.Parts;
using OrchardCore.Cms.KtuSaModule.Handlers;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Migrations;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;
using OrchardCore.Cms.KtuSaModule.Navigation;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Cms.KtuSaModule.Services;
using OrchardCore.ContentManagement;
using OrchardCore.ContentManagement.Display.ContentDisplay;
using OrchardCore.ContentManagement.Handlers;
using OrchardCore.Data.Migration;
using OrchardCore.Modules;
using OrchardCore.Navigation;
using OrchardCore.ResourceManagement;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule;

public class Startup : StartupBase
{
    public override void ConfigureServices(IServiceCollection services)
    {
        // Shared services
        services.AddScoped<IRepository, Repository>();
        services.AddTransient<IConfigureOptions<ResourceManagementOptions>, ResourceManagementOptionsConfiguration>();

        // Core content parts (shared across features)
        services.AddContentPart<ArticlePart>();
        services.AddContentPart<UserProfilePart>();
        services.AddContentPart<HeroSectionPart>();
        services.AddContentPart<StaticPagePart>();

        // Widget content parts
        services.AddContentPart<ParagraphWidgetPart>();
        services.AddContentPart<ImageWidgetPart>();
        services.AddContentPart<VideoWidgetPart>();
        services.AddContentPart<PdfDocumentWidgetPart>();
        services.AddContentPart<ImageCarouselWidgetPart>();

        // Content handlers
        services.AddScoped<IContentHandler, ContentFlowInitializationHandler>();

        services
            .AddContentPart<CategoryPart>()
            .UseDisplayDriver<CategoryPartDriver>()
            .AddHandler<CategoryPartHandler>();

        services
            .AddContentPart<CardPart>()
            .UseDisplayDriver<CardPartDriver>()
            .AddHandler<CardPartHandler>();

        services
            .AddContentPart<SaUnitPart>()
            .UseDisplayDriver<SaUnitPartDriver>();

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
            .AddContentPart<FaqPart>()
            .UseDisplayDriver<FaqPartDriver>()
            .AddHandler<FaqPartHandler>();

        services
            .AddContentPart<ActivityReportPart>()
            .UseDisplayDriver<ActivityReportPartDriver>()
            .AddHandler<ActivityReportPartHandler>();

        // Core migrations
        services.AddScoped<IDataMigration, ArticleMigrations>();
        services.AddScoped<IDataMigration, FaqMigrations>();
        services.AddScoped<IDataMigration, ContactMigrations>();
        services.AddScoped<IDataMigration, HeroSectionMigrations>();
        services.AddScoped<IDataMigration, SaUnitMigrations>();
        services.AddScoped<IDataMigration, UserMigrations>();
        services.AddScoped<IDataMigration, PositionMigrations>();
        services.AddScoped<IDataMigration, ActivityReportMigrations>();
        services.AddScoped<IDataMigration, StaticPageMigrations>();
        services.AddScoped<IDataMigration, WidgetMigrations>();

        // Core permissions
        services.AddScoped<IPermissionProvider, FaqPermissions>();
        services.AddScoped<IPermissionProvider, ArticlePermissions>();
        services.AddScoped<IPermissionProvider, HeroSectionPermissions>();
        services.AddScoped<IPermissionProvider, ContactPermissions>();
        services.AddScoped<IPermissionProvider, SaUnitPermissions>();
        services.AddScoped<IPermissionProvider, ActivityReportPermissions>();

        // Core navigation
        services.AddScoped<INavigationProvider, Navigation.AdminMenu>();
        services.AddScoped<INavigationProvider, SaUnitsInfoMenu>();
        services.AddScoped<INavigationProvider, StaticInfoMenu>();
        services.AddScoped<INavigationProvider, ArticlesMenu>();

        // Events
        services.AddHttpClient<IFientaService, FientaService>();

        services
            .AddContentPart<EventPart>()
            .UseDisplayDriver<EventPartDriver>()
            .AddHandler<EventPartHandler>();

        services.AddScoped<IDataMigration, EventMigrations>();
        services.AddScoped<IPermissionProvider, EventPermissions>();
        services.AddScoped<INavigationProvider, EventsMenu>();

        // Sponsors
        services
            .AddContentPart<SponsorPart>()
            .UseDisplayDriver<SponsorPartDriver>()
            .AddHandler<SponsorPartHandler>();

        services.AddScoped<IDataMigration, SponsorMigrations>();
        services.AddScoped<IPermissionProvider, SponsorPermissions>();
        services.AddScoped<INavigationProvider, SponsorsMenu>();

        // Documents
        services
            .AddContentPart<DocumentPart>()
            .UseDisplayDriver<DocumentPartDriver>()
            .AddHandler<DocumentPartHandler>();

        services.AddScoped<IDataMigration, DocumentMigrations>();
        services.AddScoped<IPermissionProvider, DocumentsPermissions>();
        services.AddScoped<INavigationProvider, DocumentsMenu>();
    }
}
