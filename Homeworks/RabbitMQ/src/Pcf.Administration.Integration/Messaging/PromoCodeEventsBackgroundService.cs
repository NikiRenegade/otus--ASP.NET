using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pcf.Administration.Core.Abstractions.Consumers;

namespace Pcf.Administration.Integration.Messaging;

public class PromoCodeEventsBackgroundService: BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public PromoCodeEventsBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var consumer = scope.ServiceProvider.GetRequiredService<IAdministrationEventConsumer>();
        await consumer.StartAsync();

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}