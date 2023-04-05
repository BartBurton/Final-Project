using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    void Awake()
    {
        playButton.onClick.AddListener(() =>
        {
            //Loader.Load(Loader.Scene.Lobby, false, false);
            Client();
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }

    public void Server()
    {
        GameMultiplayer.Instance.StartServer();
        Loader.Load(Loader.Scene.CharacterSelect, true);
    }
    public void Client()
    {
        GameMultiplayer.Instance.StartClient();
    }
    public void Host()
    {
        GameMultiplayer.Instance.StartHost();
        Loader.Load(Loader.Scene.CharacterSelect, true);
    }
}
