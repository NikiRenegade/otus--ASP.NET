using System;
using System.Threading.Tasks;
using Pcf.ReceivingFromPartner.Core.Abstractions.Publisher;

namespace Pcf.ReceivingFromPartner.Integration.Messaging;

public class RabbitMqAdministrationEventPublisher : IAdministrationEventPublisher
{
    private readonly IEventPublisher _eventPublisher;
    private const string ExchangeName = "administration-events";
    

    public RabbitMqAdministrationEventPublisher(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;
    }
    public Task NotifyAdminAboutPartnerManagerPromoCode(Guid partnerManagerId)
    {
        return _eventPublisher.PublishAsync(
            partnerManagerId,
            routingKey: "admin.notify",
            exchangeName: ExchangeName
        );
    }
}