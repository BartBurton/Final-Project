using System;

public class RatingTableItem : IdProperty
{
    public Guid Id { get; set; }

    public Account Account { get; set; }
    public Session Session { get; set; }

    public int Rank { get; set; }
    public double Coins { get; set; }
    public int Punches { get; set; }
    public double Exp { get; set; }
    public int Fails { get; set; }
}