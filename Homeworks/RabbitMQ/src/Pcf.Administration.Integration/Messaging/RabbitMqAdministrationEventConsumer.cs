using Pcf.Administration.Core;
using Pcf.Administration.Core.Abstractions.Consumers;

namespace Pcf.Administration.Integration.Messaging;

public class RabbitMqAdministrationEventConsumer : IAdministrationEventConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly IPromoCodeService _promoCodeService;
    private const string ExchangeName = "administration-events";

    public RabbitMqAdministrationEventConsumer(IEventConsumer consumer, IPromoCodeService promoCodeService)
    {
        _consumer = consumer;
        _promoCodeService = promoCodeService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<Guid>("admin.notify", ExchangeName, async @event =>
        {
            await _promoCodeService.UpdateAppliedPromocodesAsync(@event);
        });
        
    }
}