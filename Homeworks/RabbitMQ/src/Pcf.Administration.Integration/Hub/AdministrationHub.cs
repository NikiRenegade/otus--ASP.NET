using Pcf.Administration.Core;

namespace Pcf.Administration.Integration.Hub;

public class AdministrationHub : Microsoft.AspNetCore.SignalR.Hub
{
    private readonly IPromoCodeService _promoCodeService;

    public AdministrationHub(IPromoCodeService promoCodeService)
    {
        _promoCodeService = promoCodeService;
    }

    public async Task NotifyAdministration(Guid partnerManagerId)
    {
        await _promoCodeService.UpdateAppliedPromocodesAsync(partnerManagerId);
    }
}