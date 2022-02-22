namespace practice.domain.Kernel.Command;

public interface IResponse
{
    bool Success { get; }
    object Value { get; }

    IEnumerable<string> Errors {get;}
}