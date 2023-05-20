using System;
using System.Collections.Generic;

public class Session : IdProperty
{
    public Guid Id { get; set; }
    public Map? Map { get; set; }
    public DateTime Date { get; set; }
    public double Duration { get; set; }
    public int StartPlayersCount { get; set; }
    public string State { get; set; }
    public string GameKey { get; set; }

    public virtual ICollection<Account> Participants { get; set; }
}