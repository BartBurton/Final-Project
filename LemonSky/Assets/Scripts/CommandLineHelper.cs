using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandLineHelper : MonoBehaviour
{
    string ServerToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiU2VydmVyIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiVW5pdGVkSGVhcnRzR2FtZUB5YW5kZXgucnUiLCJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJTZXJ2ZXIiLCJleHAiOjE2ODQ0OTk5NjEsImlzcyI6IlVuaXRlZEhlYXJ0cyIsImF1ZCI6IkxlbW9uU2t5In0.BoylPoDJO1aQ3HahhLt_-Fvg-xe7Bx75JImRlXGvelM";
    public static CommandLineHelper Instance { get; private set; }
    public Dictionary<string, string> CommandLineArgs { get; private set; }
    void Awake()
    {
        Instance = this;
        CommandLineArgs = Application.isEditor ? new() : GetCommandlineArgs();
    }
    async void Start()
    {
        User.Email = "asds";
        User.Name = "Dimooooon ttututututututuu";
        Loader.Load(Loader.Scene.MainMenu);
        return;

        if (Application.isEditor)
        {
            EditorMode();
            return;
        }
        #region User
        CommandLineArgs.TryGetValue("-token", out string token);
        if(string.IsNullOrEmpty(token)) Application.Quit();
        User.Token = token;

        #endregion

        #region Mode
        if (CommandLineArgs.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    ServerMode();
                    break;
                case "client":
                    ClientMode(); 
                    break;
                default:
                    Application.Quit();
                    break;
            }
        }
        #endregion
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i];
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1] : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;
                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }

    void EditorMode()
    {
        User.Token = ServerToken;
        var api = new API(User.Token);
        Loader.Payload = async () => { User.SetUser(await api.WhoIAm()); };
        Loader.Load(Loader.Scene.MainMenu);
    }
    void ServerMode()
    {
        var api = new API(User.Token);
        Loader.Payload = async () => { User.SetUser( await api.WhoIAm()); };
        GameMultiplayer.Instance.StartServer();
    }
    void ClientMode()
    {
        var api = new API(User.Token);
        Loader.Payload = async () => { User.SetUser(await api.WhoIAm()); };
        Loader.Load(Loader.Scene.MainMenu);
    }
}