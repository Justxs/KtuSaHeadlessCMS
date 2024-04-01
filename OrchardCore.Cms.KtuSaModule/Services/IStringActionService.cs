namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IStringActionService
{
    bool IsLanguageLithuanian(string language);

    string CalculateReadingTime(string preview, string htmlBody);

    List<string> GetContentList(string htmlBody);

    string AddH1Id(string htmlBody);

}