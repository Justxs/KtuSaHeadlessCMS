using Microsoft.AspNetCore.Mvc;
using YesSql;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement.Records;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorsController(IContentManager contentManager, ISession session) : ControllerBase
{
    private static readonly string SponsorContentType = ContentTypeNames.Sponsor.ToString();

    [HttpGet]
    public async Task<ActionResult> GetSponsors()
    {
        var sponsors = await session
            .Query<ContentItem, ContentItemIndex>(index => index.ContentType == SponsorContentType && index.Published)
            .OrderByDescending(index => index.ModifiedUtc)
        .ListAsync();


        foreach (var sponsor in sponsors)
        {
            await contentManager.LoadAsync(sponsor);
        }

        var sponsorDtos = sponsors.Select(item =>
        {
            var part = item.As<SponsorPart>();
            var dto = new SponsorDto
            {
                Id = item.ContentItemId,
                Name = part.Name,
                WebsiteUrl = part.WebsiteUrl,
                LogoId = part.ImageUploadField.FileId,
            };

            return dto;
        }).ToList();

        return Ok(sponsorDtos);
    }
}