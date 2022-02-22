using System.Collections.Immutable;
using System.Collections.ObjectModel;

namespace practice.domain.Kernel.Events;



public class SubscriptionManager : ISubscriptionManager
{
    public IDictionary<string, List<Type>> Subscriptions => _handlers;
    private Dictionary<string,List<Type>> _handlers;
    

    public SubscriptionManager()
    {
        _handlers = new Dictionary<string, List<Type>>();
        
    }
    public void AddSubscription<T, TH>()
        where T : INotification
        where TH : IIntegrationEventHandler<T>
    {
        var eventTypeName = GetTypeName<T>();
        if( HasSubscribeEvent(eventTypeName)==false)
            _handlers.Add(eventTypeName, new List<Type>());
        var value = typeof(TH).Name;
        _handlers[eventTypeName].Add(typeof(TH));
    }

    public void RemoveSubscription<T,TH>()
    where T:INotification
    where TH:IIntegrationEventHandler<T>
    {
        var eventTypeName = typeof(T).Name;
        if (HasSubscribeEvent(eventTypeName)is true)
            _handlers.Remove(eventTypeName);
    }

    public static string GetTypeName<T>()
        => typeof(T).Name;

    public Type GetEventTypeByName(string eventName)
    {
        if(HasSubscribeEvent(eventName)==false)
            throw new KeyNotFoundException(nameof(eventName));
        var @event =_handlers.SingleOrDefault(t => t.Key == eventName).Key;
        return Type.GetType(@event);
    }

    public bool HasSubscribeEvent(string eventName)
        => _handlers.ContainsKey(eventName);

    public IEnumerable<Type> GetEventHandlers(string eventName)
    {
        if(string.IsNullOrEmpty(eventName))
            throw new ArgumentNullException(nameof(eventName));
        if(HasSubscribeEvent(eventName)==false)
            throw new KeyNotFoundException(nameof(eventName));
        return _handlers[eventName];
    }
}

