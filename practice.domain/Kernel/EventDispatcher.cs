using System.Linq.Expressions;
using Microsoft.Extensions.DependencyInjection;
using practice.domain.Kernel.Command;

namespace practice.domain.Kernel;

public class EventDispatcher : IDispatcher
{
    private IServiceProvider _provider;

    public EventDispatcher(IServiceProvider provider)
    {
        _provider = provider;
    }
    public Task<IResponse> Send<TRequest>(TRequest request) where TRequest : IEventRequest
    {
        var scope = _provider.CreateScope();
        
        var handler = scope.ServiceProvider.GetService<IRequestHandler<TRequest>>();
        if(handler is null)
            throw new ArgumentNullException(nameof(handler));
        return handler.Handle(request);
    }
}