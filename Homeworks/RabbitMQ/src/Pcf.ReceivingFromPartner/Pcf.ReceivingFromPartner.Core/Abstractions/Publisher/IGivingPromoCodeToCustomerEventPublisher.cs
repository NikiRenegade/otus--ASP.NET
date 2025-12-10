using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

public interface IGivingPromoCodeToCustomerEventPublisher
{
    public Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode);
}