using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode;

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
    public static void LoaderCallback()
    {
        if (targetScene.IsNetworkLoad)
            NetworkManager.Singleton.SceneManager.LoadScene(targetScene.Scene.ToString(), LoadSceneMode.Single);
        else
            SceneManager.LoadScene(targetScene.Scene.ToString());
    }
}
