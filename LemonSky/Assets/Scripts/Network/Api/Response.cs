#nullable enable
public class Response<T>
{
    public Response()
    {
        Error = default;
        Data = default;
    }
    public Response(Error? error)
    {
        Error = error;
    }

    public Response(T? data)
    {
        Data = data;
    }

    public T? Data { get; set; }
    public Error? Error { get; set; }
}