using System;
using PromoCodeFactory.Core.Domain.PromoCodeManagement;
using PromoCodeFactory.WebHost.Models;

namespace PromoCodeFactory.WebHost.Mappers;

public static class PromoCodesMapper
{
	public static PromoCodeShortResponse ToPromoCodeShortResponse(this PromoCode promoCode)
	{
		if (promoCode == null) throw new ArgumentNullException(nameof(promoCode));

		return new PromoCodeShortResponse
		{
			Id = promoCode.Id,
			Code = promoCode.Code,
			ServiceInfo = promoCode.ServiceInfo,
			BeginDate = promoCode.BeginDate.ToString(),
			EndDate = promoCode.EndDate.ToString(),
			PartnerName = promoCode.PartnerName
		};
	}
	public static PromoCode ToPromoCode(this GivePromoCodeRequest request, Preference preference)
	{
		if (request == null) throw new ArgumentNullException(nameof(request));
		return new PromoCode
		{
			ServiceInfo = request.ServiceInfo,
			PartnerName = request.PartnerName,
			Code = request.PromoCode,
			PreferenceId = preference.Id,
			Preference = preference
		};


	}
}
