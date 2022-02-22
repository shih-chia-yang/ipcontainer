namespace practice.domain.Kernel.Events;

public interface ISubscriptionManager
{
    void AddSubscription<T, TH>()
    where T : INotification
    where TH : IIntegrationEventHandler<T>;

    Type GetEventTypeByName(string eventName);

    IEnumerable<Type> GetEventHandlers(string eventName);

    bool HasSubscribeEvent(string eventName);

    void RemoveSubscription<T, TH>()
    where T : INotification
    where TH : IIntegrationEventHandler<T>;
}