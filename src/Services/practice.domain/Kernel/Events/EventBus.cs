using Microsoft.Extensions.DependencyInjection;
using practice.domain.Kernel.Command;

namespace practice.domain.Kernel.Events;

public class EventBus : IEventBus
{
    private readonly IServiceProvider _provider;
    private readonly ISubscriptionManager _subscribe;

    private readonly IDispatcher _eventDispather;

    public EventBus(
        IServiceProvider provider,
        ISubscriptionManager subscribe,
        IDispatcher dispatcher)
    {
        _provider = provider;
        _subscribe = subscribe;
        _eventDispather = dispatcher;
    }
    public async Task PublishAsync(INotification @event)
    {
        var eventName = @event.GetType().Name;
        var scopeFactory=_provider.GetRequiredService<IServiceScopeFactory>();
        if(scopeFactory is null)
            await InvokeMessageHandler(eventName, @event, _provider);
        else
        {
            using(var scope=scopeFactory.CreateScope())
            {
                await  InvokeMessageHandler(eventName, @event, scope.ServiceProvider);
            }
        }
    }

    public Task<IResponse> Send<TRequest>(TRequest @event)
    where TRequest:IEventRequest
    {
        var response = _eventDispather.Send(@event);
        return response;
    }

    private async Task InvokeMessageHandler(string eventName,INotification @event,IServiceProvider provider)
    {
        var subscriptions = _subscribe.GetEventHandlers(eventName);
        foreach(var subscription in subscriptions)
        {
            var eventType = _subscribe.GetEventTypeByName(eventName);
            var handler = _provider.GetService(subscription);
            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
            await Task.FromResult(concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event }));
        }
    }
}

