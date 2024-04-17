using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos.Documents;
using OrchardCore.Cms.KtuSaModule.Extensions;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class DocumentsController(IRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<DocumentsCategoriesDto>), 200)]
    public async Task<ActionResult> GetArticles(string language)
    {
        var documentsCategories = await repository.GetAllAsync(DocumentCategory);
        var documents = await repository.GetAllAsync(Document);
        var isLithuanian = language.IsLtLanguage();

        var categoriesDto = documentsCategories.Select(contentItem =>
        {
            var category = contentItem.As<CategoryPart>();
            var documentsCategoriesDto = new DocumentsCategoriesDto
            {
                Category = isLithuanian
                    ? category.TitleLt
                    : category.TitleEn,

                Documents = documents
                    .Where(d => d.As<DocumentPart>().CategoryField.ContentItemIds.Contains(contentItem.ContentItemId))
                    .Select(item =>
                    {
                        var document = item.As<DocumentPart>();
                        var dto = new DocumentsDto
                        {
                            Title = isLithuanian
                                ? document.TitleLt
                                : document.TitleEn,

                            PdfUrl = isLithuanian
                                ? document.DocumentLt.FileId
                                : document.DocumentEn.FileId,
                        };

                        return dto;
                    }).ToList(),
            };

            return documentsCategoriesDto;
        }).ToList();

        return Ok(categoriesDto);
    }
}