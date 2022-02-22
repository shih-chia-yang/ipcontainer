namespace practice.domain.Kernel.Events;

public interface IIntegrationEventHandler
{

}

public interface IIntegrationEventHandler<in TIntergrationEvent>:IIntegrationEventHandler
where TIntergrationEvent:INotification
{
    Task Handle(TIntergrationEvent @event);
}

