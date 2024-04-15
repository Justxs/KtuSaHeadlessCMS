using Microsoft.AspNetCore.Mvc;
using OrchardCore.Cms.KtuSaModule.Dtos;
using OrchardCore.Cms.KtuSaModule.Interfaces;
using OrchardCore.ContentManagement;
using OrchardCore.Cms.KtuSaModule.Models.Parts;
using static OrchardCore.Cms.KtuSaModule.Constants.ContentTypeConstants;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SponsorsController(IRepository repository) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<SponsorDto>), 200)]
    public async Task<ActionResult> GetSponsors()
    {
        var sponsors = await repository.GetAllAsync(Sponsor);

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