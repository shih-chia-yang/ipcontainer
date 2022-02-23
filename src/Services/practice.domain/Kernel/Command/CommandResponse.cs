namespace practice.domain.Kernel.Command;

public class CommandResponse : IResponse
{
    public bool Success { get; private set; }

    public object Value { get; private set; }

    public IEnumerable<string> Errors { get; private set; }

    public CommandResponse(bool success,object value,IEnumerable<string> errors)
    {
        Success = success;
        Value = value;
        Errors = errors;
    }
}