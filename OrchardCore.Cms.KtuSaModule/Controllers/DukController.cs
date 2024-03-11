using Microsoft.AspNetCore.Mvc;

namespace OrchardCore.Cms.KtuSaModule.Controllers;

[ApiController]
[Route("api/{language}/[controller]")]
public class DukController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> GetDuks(string language)
    {
        return Ok(language);
    }
}