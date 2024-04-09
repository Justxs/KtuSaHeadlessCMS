using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.Cms.KtuSaModule.Models.Enums;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorsController(IRepository repository) : ControllerBase
{
    private static readonly string SponsorContentType = ContentTypeNames.Sponsor.ToString();

    [HttpGet]
    public async Task<ActionResult> GetSponsors()
    {
        var sponsors = await repository.GetAllAsync(SponsorContentType);

        sponsors = sponsors.OrderByDescending(item => item.CreatedUtc);

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