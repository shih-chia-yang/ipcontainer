using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using practice.domain.Kernel.Command;
using practice.domain.unittests.Kernel;
using Xunit;

namespace practice.domain.Kernel.Events;

public class EventBus_Spec
{
    [Fact]
    public void test_publish()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IEventBus, EventBus>();
        serviceCollection.AddSingleton<ISubscriptionManager, SubscriptionManager>();
        serviceCollection.AddTransient<IIntegrationEventHandler<TestDomainEvent>, TestDomainEventHandler>();

        var provider = serviceCollection.BuildServiceProvider();
        var subscribe = provider.GetRequiredService<ISubscriptionManager>();
        var bus =provider.GetRequiredService<IEventBus>();
        subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();

        bus.PublishAsync(new TestDomainEvent());
    }

    [Fact]
    public async Task test_sendAsync()
    {
        // Given
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddSingleton<IEventBus, EventBus>();
        serviceCollection.AddSingleton<ISubscriptionManager, SubscriptionManager>();
        serviceCollection.AddTransient<IRequestHandler<TestEventRequest>, TestEventHandler>();
        var provider = serviceCollection.BuildServiceProvider();
        var subscribe = provider.GetRequiredService<ISubscriptionManager>();
        var bus =provider.GetRequiredService<IEventBus>();
        // subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();
        // When

        // var bus =provider.GetRequiredService<IEventBus>();
        var testEvent = new TestEventRequest { Message="test" };
        var handlers=await bus.Send(testEvent);
        // Then
        // Assert.IsType<IRequestHandler[]>(handlers);
        Assert.IsType<IResponse>(handlers);
    }
}