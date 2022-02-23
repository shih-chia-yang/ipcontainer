using practice.domain.Kernel.Command;

namespace practice.domain.Kernel.Events;

public interface IEventBus
{
    Task PublishAsync(INotification @event);

    Task<IResponse> Send<TRequest>(TRequest @event)
    where TRequest:IEventRequest;
}