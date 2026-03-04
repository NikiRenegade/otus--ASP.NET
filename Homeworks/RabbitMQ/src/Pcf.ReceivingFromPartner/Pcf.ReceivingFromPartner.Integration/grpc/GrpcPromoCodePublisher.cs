using System.Threading.Tasks;
using Pcf.Grpc;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Integration.grpc;

public class GrpcPromoCodePublisher : IGivingPromoCodeToCustomerEventPublisher 
{
    private readonly CustomerService.CustomerServiceClient _client;
    
    
    public GrpcPromoCodePublisher(CustomerService.CustomerServiceClient client)
    {
        _client = client;
    }

    public async Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode)
    {
        var request = new GivePromoCodeRequest
        {
            PromoCodeId = promoCode.PromoCodeId.ToString(),
            PromoCode = promoCode.PromoCode,
            PartnerId = promoCode.PartnerId.ToString(),
            PreferenceId = promoCode.PreferenceId.ToString(),
            BeginDate = promoCode.BeginDate,
            EndDate = promoCode.EndDate
        };

        await _client.GivePromoCodeAsync(request);
    }
}