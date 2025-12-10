using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Abstractions;
using Pcf.GivingToCustomer.Core.Abstractions.Consumers;
using Pcf.GivingToCustomer.Core.Domain;

namespace SurveyManageService.Infrastructure.Messaging;

public class RabbitMqGivingPromoCodeToCustomerEventConsumer : IGivingPromoCodeToCustomerEventConsumer
{
    private readonly IEventConsumer _consumer;
    private readonly IPromoCodeService _promoCodeService;
    private const string ExchangeName = "promocode-events";

    public RabbitMqGivingPromoCodeToCustomerEventConsumer(IEventConsumer consumer, IPromoCodeService promoCodeService)
    {
        _consumer = consumer;
        _promoCodeService = promoCodeService;
    }

    public async Task StartAsync()
    {
        await _consumer.SubscribeAsync<GivePromoCodeReq>("promocode.give", ExchangeName, async @event =>
        {
            await _promoCodeService.GivePromoCodesToCustomersWithPreferenceAsync(@event);
        });
    }
}