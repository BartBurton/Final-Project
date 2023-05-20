using System;

public class Error
{
    public Error()
    {
        Id = Guid.NewGuid();
        Message = "";
    }
    public Error(string message)
    {
        Id = Guid.NewGuid();
        Message = message;
    }

    public Error(Guid id, string message)
    {
        Id = id;
        Message = message;
    }

    public Guid Id { get; set; }
    public string Message { get; set; }
}