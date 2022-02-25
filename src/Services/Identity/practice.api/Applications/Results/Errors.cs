namespace practice.api.Applications.Results;

public class ResponseError
{
    public int Code { get; set; }

    public string Type { get; set; }

    public string Message { get; set; }

    internal ResponseError(int code,string message,string type)
    {
        Code=code;
        Message = message;
        Type = type;
    }

    public static ResponseError CreateNew(int code, string message, string type)
        => new ResponseError(code, message, type);
}