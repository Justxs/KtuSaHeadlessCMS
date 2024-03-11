namespace OrchardCore.Cms.KtuSaModule.Dtos;

public class DukDto
{
    public string Id { get; set; } = null!;

    public string Question { get; set; } = null!;

    public string Answer { get; set; } = null!;

    public DateTime ModifiedDate { get; set; }
}