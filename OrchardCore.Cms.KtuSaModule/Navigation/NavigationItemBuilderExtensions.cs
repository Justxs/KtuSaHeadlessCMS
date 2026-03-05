using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;
using OrchardCore.Security.Permissions;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public static class NavigationItemBuilderExtensions
{
    private const string AdminNavigationName = "admin";
    private const string OrchardCoreContentsArea = "OrchardCore.Contents";
    private const string SolidIconClass = "icon-class-fas";

    public static bool IsAdminMenu(this string name) =>
        string.Equals(name, AdminNavigationName, StringComparison.OrdinalIgnoreCase);

    public static NavigationItemBuilder WithIcon(this NavigationItemBuilder builder, string iconClass) =>
        builder
            .AddClass(iconClass)
            .AddClass(SolidIconClass);

    public static NavigationItemBuilder AddContentList(
        this NavigationItemBuilder builder,
        LocalizedString text,
        string contentTypeId,
        Permission permission)
    {
        builder.Add(text, item => item
            .Action("List", "Admin", new
            {
                area = OrchardCoreContentsArea,
                contentTypeId
            })
            .Permission(permission)
            .WithIcon("icon-class-fa-list"));

        return builder;
    }

    public static NavigationBuilder AddContentList(
        this NavigationBuilder builder,
        LocalizedString text,
        string position,
        string contentTypeId,
        Permission permission) =>
        builder.Add(text, position, item => item
            .Action("List", "Admin", new
            {
                area = OrchardCoreContentsArea,
                contentTypeId
            })
            .Permission(permission)
            .WithIcon("icon-class-fa-list"));

    public static NavigationItemBuilder AddCreateContentType(
        this NavigationItemBuilder builder,
        LocalizedString text,
        string contentTypeId,
        Permission permission,
        string iconClass = "icon-class-fa-circle-plus")
    {
        builder.Add(text, item => item
            .Url($"/Admin/Contents/ContentTypes/{contentTypeId}/Create")
            .Permission(permission)
            .WithIcon(iconClass));

        return builder;
    }
}
