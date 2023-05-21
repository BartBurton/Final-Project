public static class User
{
    static User()
    {
#warning ���������� default ������-����� ��� ����������. ���������� ������� ���, ����� �������� ������� � �������������
        Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiTG9uZVdhbGQiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJib25kYXJldi5ib2dkYW4yMDEzQHlhbmRleC5ydSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjoxNjg0ODY2OTQ5LCJpc3MiOiJVbml0ZWRIZWFydHMiLCJhdWQiOiJMZW1vblNreSJ9.IvD6I2jUeOKAKbib2KZl9iGS980kKY9XpYZq_KMTUN0";
        Name = "unknown";
    }
    public static string Email { get; set; }
    public static string Name { get; set; }
    public static string Password { get; set; }
    public static string Token { get; set; }

    public static double Cash { get; set; }

    public static void SetUser(Account acc)
    {
        User.Name = acc.Name;
        User.Email = acc.Email;
        User.Password = acc.Password;
        User.Cash = acc.Cash.Current;
    }
}
