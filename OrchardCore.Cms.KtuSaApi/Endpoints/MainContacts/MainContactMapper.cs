using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public static class MainContactMapper
{
    public static MainContactResponse ToResponse(this ContentItem item)
    {
        var addressPart = item.As<AddressPart>();
        var contactPart = item.As<ContactPart>();
        return new MainContactResponse
        {
            Address = addressPart.Address,
            Email = contactPart.Email,
            PhoneNumber = contactPart.PhoneNumber
        };
    }
}