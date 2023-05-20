using System;

public class Map : IdProperty
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string GameKey { get; set; }
    public string Description { get; set; }
    public int MaxPlayersCount { get; set; }
}
