using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;
using Pcf.ReceivingFromPartner.Core.Domain;

namespace Pcf.ReceivingFromPartner.Integration.Messaging;

public class RabbitMqGivingPromoCodeToCustomerEventPublisher: IGivingPromoCodeToCustomerEventPublisher
{
    
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "promocode-events";
    

    public RabbitMqGivingPromoCodeToCustomerEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    public Task GivePromoCodeToCustomer(PromoCodeGivenEvent promoCode)
    {
        return _eventPublisher.PublishAsync(
            promoCode,
            routingKey: "promocode.give",
            exchangeName: ExchangeName
        );
    }
}