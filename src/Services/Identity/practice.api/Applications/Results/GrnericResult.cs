namespace practice.api.Applications.Results;

public class Response<TResult>
{
    public TResult Content { get; set; }
    public ResponseError Error { get; private set; }

    public bool IsSuccess => Error == null;

    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;

    public Response(ResponseError error)
    {
        Error = error;
    }
}


public class ApiResponse<TResult>:Response<List<TResult>>
{
    public ApiResponse(ResponseError error) : base(error)
    {
    }

    public int Page { get; set; }

    public int ResultCount { get; set; }

    public int Results { get; set; }
}