public static class User
{
    static User()
    {
#warning ���������� default ������-����� ��� ����������. ���������� ������� ���, ����� �������� ������� � �������������
        Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU2VydmVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiVW5pdGVkSGVhcnRzR2FtZUB5YW5kZXgucnUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTZXJ2ZXIiLCJleHAiOjE2ODQ5NTk5NzIsImlzcyI6IlVuaXRlZEhlYXJ0cyIsImF1ZCI6IkxlbW9uU2t5In0.eHxseUqV1zBNajs2sw0ov5qPlwOuPWPkf6OnvrfkMUQ";
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
