using System;
using System.Collections.Generic;
using System.Linq;
using practice.domain.Kernel.Events;
using Xunit;

namespace practice.domain.unittests.Kernel;

public class SubscriptionManager_Spec
{
    [Fact]
    public void test_AddSubscription()
    {
        var subscribe = new SubscriptionManager();
        subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();

        Assert.True(subscribe.Subscriptions.Count() > 0);
    }

    [Fact]
    public void test_HasSubscribeEvent()
    {
        var subscribe = new SubscriptionManager();
        var eventName = typeof(TestDomainEvent).Name;
        subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();

        var hasTargetEvent = subscribe.HasSubscribeEvent(eventName);

        Assert.True(hasTargetEvent);
    }

    [Fact]
    public void test_RemoveSubscription()
    {
        var subscribe = new SubscriptionManager();
        var eventName = typeof(TestDomainEvent).Name;
        subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();

        subscribe.RemoveSubscription<TestDomainEvent, TestDomainEventHandler>();

        Assert.Equal(0,subscribe.Subscriptions.Count());
    }

    public void test_GetEventHandlers()
    {
        var subscribe = new SubscriptionManager();
        var eventName = typeof(TestDomainEvent).Name;
        subscribe.AddSubscription<TestDomainEvent, TestDomainEventHandler>();

        var list = subscribe.GetEventHandlers(eventName);
        Assert.IsType<IEnumerable<Type>>(list);
    }
}