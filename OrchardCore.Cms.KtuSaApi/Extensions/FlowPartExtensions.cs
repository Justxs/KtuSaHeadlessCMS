using OrchardCore.Cms.KtuSaApi.Endpoints.Shared;
using OrchardCore.Cms.KtuSaModule.Models;
using OrchardCore.Cms.KtuSaModule.Models.Parts.Widgets;
using OrchardCore.ContentManagement;
using OrchardCore.Flows.Models;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;
using static OrchardCore.Cms.KtuSaModule.Constants.RegexConstants;

namespace OrchardCore.Cms.KtuSaApi.Extensions;

public static class FlowPartExtensions
{
    extension(ContentItem item)
    {
        public List<ContentBlockResponse> ToContentBlocks(Language language, IMediaFileStore mediaFileStore)
        {
            return item.ToContentBlocks(language.FlowPartName, mediaFileStore);
        }

        public List<ContentBlockResponse> ToContentBlocks(string flowPartName, IMediaFileStore mediaFileStore)
        {
            var flow = item.Get<FlowPart>(flowPartName);

            if (flow?.Widgets is null or { Count: 0 })
                return [];

            return
            [
                .. flow.Widgets
                    .Select(widget => widget.ToContentBlock(mediaFileStore))
                    .Where(block => block is not null)!
            ];
        }

        public string GetCombinedHtml(Language language)
        {
            var flow = item.Get<FlowPart>(language.FlowPartName);

            if (flow?.Widgets is null or { Count: 0 })
                return string.Empty;

            return string.Join("",
                flow.Widgets
                    .Where(w => w.ContentType == ParagraphWidget)
                    .Select(w => w.As<ParagraphWidgetPart>()?.Body?.Html)
                    .Where(html => !string.IsNullOrEmpty(html)));
        }

        public List<string> GetContentList(Language language)
        {
            var combinedHtml = item.GetCombinedHtml(language);

            if (string.IsNullOrEmpty(combinedHtml))
                return [];

            return
            [
                .. HtmlHeadingTagRegex()
                    .Matches(combinedHtml)
                    .Select(m => HtmlTagRemoveRegex().Replace(m.Groups[1].Value, string.Empty).Trim())
                    .Where(text => !string.IsNullOrEmpty(text))
            ];
        }

        private ContentBlockResponse? ToContentBlock(IMediaFileStore mediaFileStore)
        {
            return item.ContentType switch
            {
                ParagraphWidget => new ContentBlockResponse
                {
                    Type = "paragraph",
                    Html = item.As<ParagraphWidgetPart>()?.Body?.Html
                },
                ImageWidget => new ContentBlockResponse
                {
                    Type = "image",
                    ImageUrl = item.As<ImageWidgetPart>()?.Image?.ToPublicUrl(mediaFileStore)
                },
                VideoWidget => new ContentBlockResponse
                {
                    Type = "video",
                    VideoUrl = item.As<VideoWidgetPart>()?.Url.Text
                },
                PdfDocumentWidget => new ContentBlockResponse
                {
                    Type = "pdf",
                    PdfUrl = item.As<PdfDocumentWidgetPart>()?.Document?.ToPublicUrl(mediaFileStore)
                },
                ImageCarouselWidget => new ContentBlockResponse
                {
                    Type = "carousel",
                    ImageUrls = item.As<ImageCarouselWidgetPart>()?.Images?.ToPublicUrls(mediaFileStore)
                },
                _ => null
            };
        }
    }
}