namespace practice.domain.Kernel;


public interface IHandlerFactory
{

}

public class HandlerFactory:IHandlerFactory
{
    private readonly IServiceProvider _provider;

    public HandlerFactory(IServiceProvider provider)
    {
        _provider = provider;
    }
}