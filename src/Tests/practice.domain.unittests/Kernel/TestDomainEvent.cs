using practice.domain.Kernel.Events;

namespace practice.domain.unittests.Kernel;

public class TestDomainEvent:INotification
{
    public string Message { get; set; }
}