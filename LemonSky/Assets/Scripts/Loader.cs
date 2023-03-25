using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene{
        MainMenu,
        Game,
        Loading
    }
    public static int targetSceneIndex;

    public static void Load(Scene targetScene){
        SceneManager.LoadScene(targetScene.ToString());
    }
}
