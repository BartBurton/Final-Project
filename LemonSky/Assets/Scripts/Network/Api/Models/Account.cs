using System;
using System.Collections.Generic;

public class Account : IdProperty
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public int Role { get; set; }
    public bool EmailConfirmed { get; set; }
    public bool Active { get; set; }
    public string State { get; set; }

    public Cash Cash { get; set; }
    public AccountStatistic Statistic { get; set; }
    public IEnumerable<Session> Sessions { get; set; }
    public IEnumerable<Stuff> Stuffs{ get; set; }
}