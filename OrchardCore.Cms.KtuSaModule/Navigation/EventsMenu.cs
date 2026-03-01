using Microsoft.Extensions.Localization;
using OrchardCore.Cms.KtuSaModule.Permissions;
using OrchardCore.Navigation;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class EventsMenu(IStringLocalizer<EventsMenu> stringLocalizer) : INavigationProvider
{
    private readonly IStringLocalizer T = stringLocalizer;

    public ValueTask BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        if (!string.Equals(name, "admin", StringComparison.OrdinalIgnoreCase)) return ValueTask.CompletedTask;

        builder.Add(T["Events"], content => content
            .AddClass("icon-class-fa-calendar-days")
            .AddClass("icon-class-fas")
            .Add(T["All events"], eventContentType => eventContentType
                .Action("List", "Admin", new
                {
                    area = "OrchardCore.Contents",
                    contentTypeId = Event
                })
                .Permission(EventPermissions.ManageEvents)
                .AddClass("icon-class-fa-list")
                .AddClass("icon-class-fas"))
            .Add(T["Create an event"], createAction => createAction
                .Url($"/Admin/Contents/ContentTypes/{Event}/Create")
                .Permission(EventPermissions.ManageEvents)
                .AddClass("icon-class-fa-calendar-plus")
                .AddClass("icon-class-fas"))
        );

        return ValueTask.CompletedTask;
    }
}