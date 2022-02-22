using System;
using System.Threading.Tasks;
using practice.domain.Kernel.Events;

namespace practice.domain.unittests.Kernel;

public class TestDomainEventHandler : IIntegrationEventHandler<TestDomainEvent>
{
    public TestDomainEventHandler()
    {
        
    }
    public Task Handle(TestDomainEvent @event)
    {
        Console.WriteLine(nameof(TestDomainEventHandler));
        Console.WriteLine(@event.Message);
        Console.ReadLine();
        return Task.FromResult(true);
    }
}
