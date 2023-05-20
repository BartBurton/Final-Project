public static class User
{
    public static string Email {get; set;}
    public static string Name {get; set;} = "unknown";
    public static string Password {get; set;}
    public static string Token {get; set;}

    public static void SetUser(Account acc)
    {
        User.Name = acc.Name;
        User.Email = acc.Email;
        User.Password = acc.Password;
    }
}
