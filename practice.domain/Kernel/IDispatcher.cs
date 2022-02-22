using practice.domain.Kernel.Command;

namespace practice.domain.Kernel;

public interface IDispatcher
{
    Task<IResponse> Send<TRequest>(TRequest request)
    where TRequest : IEventRequest;

    // Task<IResponse> Send(Type @event);
}