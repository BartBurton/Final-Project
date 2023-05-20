using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI errorText;

    void Awake()
    {
        errorText.enabled = false;
        GameMultiplayer.Instance.OnFailJoinGame += GameMultiplayer_OnFailJoinGame;
    }

    private void FixedUpdate()
    {
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
    public void Store()
    {
        Loader.Load(Loader.Scene.Store, false, false);
    }
    public void Quit()
    {
        Application.Quit();
    }

    IEnumerator HideError()
    {
        errorText.enabled = true;
        yield return new WaitForSeconds(10);
        errorText.enabled = false;
    }

    void GameMultiplayer_OnFailJoinGame(object sender, EventArgs e)
    {
        StartCoroutine(HideError());
    }

    void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailJoinGame -= GameMultiplayer_OnFailJoinGame;
        Debug.Log("Удалили карутину");
    }

}
