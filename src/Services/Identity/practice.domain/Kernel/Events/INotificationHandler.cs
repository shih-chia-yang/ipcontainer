using practice.domain.Kernel.Command;

namespace practice.domain.Kernel.Events;

public interface INotificationHandler
{

}

public interface INotificationHandler<in TNotification>:INotificationHandler
where TNotification:INotification
{
    Task Handle(TNotification request);
}


public interface INotificationHandler<in TNotification,out TResponse>:INotificationHandler
where TNotification:INotification
{
    Task<IResponse> Handle(TNotification request);
}