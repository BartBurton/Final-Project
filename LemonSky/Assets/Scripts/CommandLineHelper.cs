using System.Collections.Generic;
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
    void Start()
    {
        if (Application.isEditor)
            Loader.Load(Loader.Scene.MainMenu);

        #region User
        if (CommandLineArgs.TryGetValue("-login", out string login))
            User.Email = login;
        if (CommandLineArgs.TryGetValue("-name", out string name))
            User.Name = name.Replace("_", " ");
        if (CommandLineArgs.TryGetValue("-password", out string password))
            User.Password = password;
        Debug.Log(name);
        #endregion

        #region Mode
        if (CommandLineArgs.TryGetValue("-mode", out string mode))
        {
            switch (mode)
            {
                case "server":
                    GameMultiplayer.Instance.StartServer();
                    break;
                case "client":
                    Loader.Load(Loader.Scene.MainMenu);
                    break;
                default:
                    Application.Quit();
                    break;
            }
        }
        else
            Application.Quit();
        #endregion
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();
        var args = System.Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;
                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}