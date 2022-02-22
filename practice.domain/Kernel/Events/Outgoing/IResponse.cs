namespace practice.domain.Kernel.Events.Outgoing;

public interface IEventResponse
{
    bool Success { get; }
    Type ResponseType{get; }

    object Value { get; }
}