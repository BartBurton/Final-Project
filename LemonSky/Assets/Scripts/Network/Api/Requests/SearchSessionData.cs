using System;

public class SearchSessionData : IRequest
{
    public Guid MapId { get; set; }
    public int Duration{ get; set; }
}