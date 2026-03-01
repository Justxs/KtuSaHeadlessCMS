using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using OrchardCore.Media;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class GetDocumentsEndpoint(IRepository repository, IMediaFileStore mediaFileStore)
    : Endpoint<GetDocumentsRequest, List<DocumentCategoryResponse>>
{
    public override void Configure()
    {
        Get("api/documents");
        AllowAnonymous();
        ResponseCache(300);
        Description(b => b
            .WithTags("Documents")
            .WithSummary("Get documents grouped by category")
            .WithDescription(
                "Returns all documents grouped by their category. " +
                "Each entry contains a category name and a list of documents with titles and PDF file URLs. " +
                "Pass language=lt or language=en.")
            .Produces<List<DocumentCategoryResponse>>(200));
    }

    public override async Task HandleAsync(GetDocumentsRequest req, CancellationToken ct)
    {
        var documentsCategories = await repository.GetAllAsync(DocumentCategory);
        var documents = await repository.GetAllAsync(Document);
        var isLithuanian = req.Language.IsLtLanguage();

        var response = documentsCategories
            .Select(item => item.ToCategoryResponse(isLithuanian, documents, mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
