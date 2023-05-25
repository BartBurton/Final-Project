using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using System;
using System.Threading.Tasks;

public static class Loader
{
    public enum Scene
    {
        MainMenu = 1,
        Game = 2,
        Loading = 3,
        InfinityLoading = 4,
        CharacterSelect = 5,
        Store = 6,
        MatchResults = 7,
    }
    struct SceneManagment
    {
        public Scene Scene;
        public bool IsNetworkLoad;
    }

    static SceneManagment targetScene;
    public static event Func<Task> BeforeLoad;
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
        await BeforeLoad.Raise();
        BeforeLoad = null;
        if (targetScene.IsNetworkLoad)
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.Scene.ToString(), LoadSceneMode.Single);
        else
            SceneManager.LoadScene(targetScene.Scene.ToString());
    }
}
