using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CommandLineHelper : MonoBehaviour
{
    public static CommandLineHelper Instance { get; private set; }
    public Dictionary<string, string> CommandLineArgs { get; private set; }
    void Awake()
    {
        Instance = this;
        CommandLineArgs = Application.isEditor ? new() : GetCommandlineArgs();
    }
    async void Start()
    {
        if (Application.isEditor)
        {
            ClientMode();
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

    void ServerMode()
    {
        Loader.BeforeLoad += async () => { User.SetUser(await APIRequests.WhoIAm()); };
        GameMultiplayer.Instance.StartServer();
    }
    void ClientMode()
    {
        Loader.BeforeLoad += async () => { User.SetUser(await APIRequests.WhoIAm()); Debug.Log(User.Name); };
        Loader.Load(Loader.Scene.MainMenu);
    }
}