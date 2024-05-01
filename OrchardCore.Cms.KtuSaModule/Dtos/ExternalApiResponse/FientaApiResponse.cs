namespace OrchardCore.Cms.KtuSaModule.Dtos.ExternalApiResponse;

public class FientaApiResponse
{
    public Dictionary<string, object> Success { get; set; } = null!;

    public Dictionary<string, object> Time { get; set; } = null!;

    public int Count { get; set; }

    public List<FientaEvent> Events { get; set; } = null!;
}