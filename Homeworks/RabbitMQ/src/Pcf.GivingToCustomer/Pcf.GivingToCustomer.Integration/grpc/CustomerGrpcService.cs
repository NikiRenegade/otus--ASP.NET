using System;
using System.Threading.Tasks;
using Grpc.Core;
using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.Grpc;

namespace Pcf.GivingToCustomer.Integration.grpc;

public class CustomerGrpcService: CustomerService.CustomerServiceBase
{
    private readonly IPromoCodeService _promoCodeService;

    public CustomerGrpcService(IPromoCodeService promoCodeService)
    {
        _promoCodeService = promoCodeService;
    }

    public override async Task<GivePromoCodeResponse> GivePromoCode(
        GivePromoCodeRequest request,
        ServerCallContext context)
    {
        var model = new GivePromoCodeReq
        {
            PromoCodeId = Guid.Parse(request.PromoCodeId),
            PromoCode = request.PromoCode,
            PartnerId = Guid.Parse(request.PartnerId),
            PreferenceId = Guid.Parse(request.PreferenceId),
            BeginDate = request.BeginDate,
            EndDate = request.EndDate
        };

        await _promoCodeService.GivePromoCodesToCustomersWithPreferenceAsync(model);

        return new GivePromoCodeResponse
        {
            Success = true
        };
    }
}