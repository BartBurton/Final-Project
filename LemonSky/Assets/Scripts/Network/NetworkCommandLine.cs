using System.Collections.Generic;
using System;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class NetworkCommandLine : MonoBehaviour
{
    private NetworkManager netManager;

    void Start()
    {
        netManager = GetComponentInParent<NetworkManager>();
        if (Application.isEditor) return;
        var args = GetCommandlineArgs();
        #region Mode
        // if (args.TryGetValue("-mode", out string mode))
        // {
        //     switch (mode)
        //     {
        //         case "server":
        //             netManager.StartServer();
        //             break;
        //         case "host":
        //             netManager.StartHost();
        //             break;
        //         case "client":
        //             netManager.StartClient();
        //             break;
        //     }
        // }
        #endregion
        #region Login
        if (args.TryGetValue("-login", out string login))
        {
            //label.text = login;
        }
        else {
            //Application.Quit();
        }
        Debug.Log("CommandLine");
        if (args.TryGetValue("-name", out string name))
            User.Name = name.Replace("_", " ");
        Debug.Log(name);
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