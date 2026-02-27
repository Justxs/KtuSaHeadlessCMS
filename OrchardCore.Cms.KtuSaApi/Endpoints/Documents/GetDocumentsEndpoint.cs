using FastEndpoints;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.Documents;

public class GetDocumentsEndpoint(IRepository repository)
    : Endpoint<GetDocumentsRequest, List<DocumentCategoryResponse>>
{
    public override void Configure()
    {
        Get("api/{language}/Documents");
        AllowAnonymous();
        Description(b => b
            .WithTags("Documents")
            .WithSummary("Get documents grouped by category")
            .WithDescription(
                "Returns all documents grouped by their category. Each entry contains a category name and a list of documents with titles and PDF file URLs. Language: 'lt' or 'en'.")
            .Produces<List<DocumentCategoryResponse>>(200));
    }

    public override async Task HandleAsync(GetDocumentsRequest req, CancellationToken ct)
    {
        var documentsCategories = await repository.GetAllAsync(DocumentCategory);
        var documents = await repository.GetAllAsync(Document);
        var isLithuanian = req.Language.IsLtLanguage();

        var categoriesDto = documentsCategories
            .Select(item => item.ToCategoryResponse(isLithuanian, documents))
            .ToList();

        await Send.OkAsync(categoriesDto, ct);
    }
}