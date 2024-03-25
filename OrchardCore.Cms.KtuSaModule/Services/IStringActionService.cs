namespace OrchardCore.Cms.KtuSaModule.Services;

public interface IStringActionService
{
    bool IsLanguageLithuanian(string language);

    string CalculateReadingTime(string preview, string htmlBody);
}