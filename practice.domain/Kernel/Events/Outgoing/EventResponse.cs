namespace practice.domain.Kernel.Events.Outgoing;

public class EventResponse : IEventResponse
{
    public bool Success { get; private set; }

    public Type ResponseType { get; private set; }

    public object Value { get; private set; }

    public EventResponse(bool success,Type type,object value)
    {
        Success = success;
        ResponseType = type;
        Value = value;
    }
}

