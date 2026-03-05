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
        if (!name.IsAdminMenu()) return ValueTask.CompletedTask;

        builder.Add(T["Events"], content => content
            .WithIcon("icon-class-fa-calendar-days")
            .AddContentList(T["All events"], Event, EventPermissions.ManageEvents)
            .AddCreateContentType(
                T["Create an event"],
                Event,
                EventPermissions.ManageEvents,
                "icon-class-fa-calendar-plus")
        );

        return ValueTask.CompletedTask;
    }
}
