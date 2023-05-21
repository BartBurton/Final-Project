using System.Collections.Generic;

public class SessionUpdateData : IRequest
{
    public string SessionId { get; set; }
    public string State { get; set; }
    public IEnumerable<PlayerResult>? Participants { get; set; }
}