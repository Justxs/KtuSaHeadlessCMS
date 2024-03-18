using Microsoft.Extensions.Localization;
using OrchardCore.Navigation;

namespace OrchardCore.Cms.KtuSaModule.Navigation;

public class PresidentMenu(IStringLocalizer T) : INavigationProvider
{
    public async Task BuildNavigationAsync(string name, NavigationBuilder builder)
    {
        //await builder
        //    .AddAsync(
        //        T["President menu"], 
        //        "5", 
        //        item => item
        //            .Action());


        return ;
    }
}