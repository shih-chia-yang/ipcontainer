namespace practice.domain.Kernel.Events;

public interface IEventHandler<TRequest>
where TRequest:IEventRequest
{
    void Handle(TRequest request);
}

public interface IEventHandler<TRequest,TResponse>
where TRequest:IEventRequest
{
     Task<TResponse> Handle(TRequest request);
}