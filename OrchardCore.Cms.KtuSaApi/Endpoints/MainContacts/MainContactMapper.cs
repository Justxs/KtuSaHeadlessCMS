using OrchardCore.Cms.KtuSaModule.Models.Parts;
using OrchardCore.ContentManagement;

namespace OrchardCore.Cms.KtuSaApi.Endpoints.MainContacts;

public static class MainContactMapper
{
    extension(ContentItem item)
    {
        public MainContactResponse ToResponse()
        {
        var addressPart = item.GetOrCreate<AddressPart>();
        var contactPart = item.GetOrCreate<ContactPart>();
            return new MainContactResponse
            {
                Address = addressPart.Address,
                Email = contactPart.Email,
                PhoneNumber = contactPart.PhoneNumber
            };
        }
    }
}
