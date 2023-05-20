using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;
using System.Threading.Tasks;

public static class Loader
{
    public enum Scene
    {
        MainMenu,
        Game,
        Loading,
        Lobby,
        CharacterSelect
    }
    struct SceneManagment
    {
        public Scene Scene;
        public bool IsNetworkLoad;
    }

    static SceneManagment targetScene;
    public static event Func<object, EventArgs, Task> OnLoad;

    public static Func<Task> Payload;
    public static void Load(Scene targetScene, bool isNetLoad = false, bool fakeTime = true)
    {
        Loader.targetScene = new SceneManagment()
        {
            Scene = targetScene,
            IsNetworkLoad = isNetLoad
        };
        if (fakeTime)
            SceneManager.LoadScene(Scene.Loading.ToString());
        else
            LoaderCallback();
    }
    public static void NetworkLoad(Scene targetScene)
    {
        Loader.targetScene = new SceneManagment()
        {
            Scene = targetScene,
            IsNetworkLoad = true
        };
        LoaderCallback();
    }
    public static async Task LoaderCallback()
    {
        var payload = Payload?.Invoke();
        Payload = null;
        if (payload != null)
            await payload;
        if (targetScene.IsNetworkLoad)
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.Scene.ToString(), LoadSceneMode.Single);
        else
            SceneManager.LoadScene(targetScene.Scene.ToString());
        var a = User.Email;
        Debug.Log(a);
    }
}
