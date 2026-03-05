using FastEndpoints;
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
        Description(b => b
            .WithTags("Documents")
            .WithSummary("Get documents grouped by category")
            .WithDescription(
                "Returns published documents grouped by category. " +
                "Each group contains a localized category title and localized documents with PDF URLs. " +
                "Use query parameter language=en (default) or language=lt.")
            .Produces<List<DocumentCategoryResponse>>(200)
            .ProducesProblem(400));
    }

    public override async Task HandleAsync(GetDocumentsRequest req, CancellationToken ct)
    {
        var documentsCategories = await repository.GetAllAsync(DocumentCategory);
        var documents = await repository.GetAllAsync(Document);

        var response = documentsCategories
            .Select(item => item.ToCategoryResponse(req.Language, documents, mediaFileStore))
            .ToList();

        await Send.OkAsync(response, ct);
    }
}
