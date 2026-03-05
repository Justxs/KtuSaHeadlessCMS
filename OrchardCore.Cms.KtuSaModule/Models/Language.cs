namespace OrchardCore.Cms.KtuSaModule.Models;

public enum Language { LT, EN }

public static class LanguageExtensions
{
    extension(Language language)
    {
        public T Resolve<T>(T lt, T en) => language == Language.LT ? lt : en;

        public string FlowPartName => language == Language.LT ? "ContentLt" : "ContentEn";

        public string FaqAnswerFlowPartName => language == Language.LT ? "AnswerLt" : "AnswerEn";
    }
}
