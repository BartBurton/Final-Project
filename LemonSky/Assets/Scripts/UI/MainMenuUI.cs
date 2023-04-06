using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] TextMeshProUGUI errorText;

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
        errorText.enabled = false;
        GameMultiplayer.Instance.OnFailJoinGame += GameMultiplayer_OnFailJoinGame;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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
    }

    void OnDestroy(){
        GameMultiplayer.Instance.OnFailJoinGame -= GameMultiplayer_OnFailJoinGame;
        Debug.Log("Удалили карутину");
    }
    void GameMultiplayer_OnFailJoinGame(object sender, EventArgs e){
        StartCoroutine(HideError());
    }

    IEnumerator HideError()
    {
        errorText.enabled = true;
        yield return new WaitForSeconds(5);
        errorText.enabled = false;
    }
}
