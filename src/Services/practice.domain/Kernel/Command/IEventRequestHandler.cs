namespace practice.domain.Kernel.Command;

public interface IRequestHandler
{

}

public interface IRequestHandler<in TRequest>:IRequestHandler
where TRequest:IEventRequest
{
    Task<IResponse> Handle(TRequest request);
}

public interface IRequestHandler<in TRequest,out TResponse>:IRequestHandler
where TRequest:IEventRequest
where TResponse:IResponse
{
     Task<IResponse> Handle(TRequest request);
}