using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;

public class MainMenuUI : MonoBehaviour, IShowErrorMessage
{
    [SerializeField] TextMeshProUGUI errorText;
    [SerializeField] Button SearchButton;
    [SerializeField] GameObject StopSearchPanel;
    [SerializeField] Button StopSearchButton;
    [SerializeField] TextMeshProUGUI SearchTimer;

    Guid _findedSession = Guid.Empty; IEnumerator coroutine;
    bool _isSearch = false;

    void Awake()
    {
        SearchButton.gameObject.SetActive(true);
        StopSearchPanel.SetActive(false);
        SearchButton.onClick.AddListener(SearchClick);
        StopSearchButton.onClick.AddListener(StopSearchClick);
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
    public void LoadStore()
    {
        Loader.BeforeLoad += async () => { Store.Stuffs = await APIRequests.GetStuffs(); };
        Loader.Load(Loader.Scene.Store, false);
    }
    public void Quit()
    {
        Application.Quit();
    }


    void GameMultiplayer_OnFailJoinGame(object sender, EventArgs e)
    {
        ShowError("К сожалению, все сервера заполнены\r\nПопробуйте снова через пару минут");
    }

    void OnDestroy()
    {
        GameMultiplayer.Instance.OnFailJoinGame -= GameMultiplayer_OnFailJoinGame;
    }

    async void SearchClick()
    {
        InSearch();
        var map = (await APIRequests.GetMaps()).ElementAtOrDefault(0);
        if (map is null)
        {
            OutSearch();
            ShowError("Выбранная карта не найдена");
            return;
        }
        if (!_isSearch) return;
        await Task.Delay(new System.Random().Next(2000, 7000));
        if (!_isSearch) return;
        var session = await APIRequests.SearchSession(map.Id, 600);
        if (session is null)
        {
            OutSearch();
            ShowError("К сожалению, при поиске чтото пошло не так\r\nПопробуйте снова через пару минут");
            return;
        }
        Debug.Log("Найдена сессия");
        if (!_isSearch) return;
        _findedSession = session.Id;
        var count = 0;
        while (string.IsNullOrEmpty(session.GameKey) && _isSearch)
        {
            session = await APIRequests.StatusSession(_findedSession);
            if (!_isSearch) return;
            Debug.Log("Сессия обновлена " + ++count + " раз");
            await Task.Delay(2000);
        }
        if (!_isSearch) return;
#warning Не реализовано подключение по session.GameKey
        GameMultiplayer.Instance.StartClient();
    }
    async void StopSearchClick()
    {
        OutSearch();
        var id = _findedSession;
        _findedSession = Guid.Empty;
        await APIRequests.StopSearchSession(id);
    }

    IEnumerator StartSearchTimer()
    {
        var time = 0d;
        SearchTimer.text = $"{time / 60}:{time % 60}";
        while (true)
        {
            yield return new WaitForSeconds(1);
            time += 1d;
            SearchTimer.text = $"{(int)(time / 60)}:{(int)(time % 60)}";
        }
    }

    void InSearch()
    {
        _isSearch = true;
        SearchButton.gameObject.SetActive(false);
        StopSearchPanel.SetActive(true);
        coroutine = StartSearchTimer();
        StartCoroutine(coroutine);
        Debug.Log("Начался поиск");
    }
    void OutSearch()
    {
        _isSearch = false;
        StopSearchPanel.SetActive(false);
        SearchButton.gameObject.SetActive(true);
        StopCoroutine(coroutine);
        Debug.Log("Поиск отменен");
    }

    public void ShowError(string textToShow)
    {
        StartCoroutine(HideError("Выбраннвя карта не найдена"));
    }
    IEnumerator HideError(string textToShow)
    {
        errorText.text = textToShow;
        errorText.enabled = true;
        yield return new WaitForSeconds(10);
        errorText.enabled = false;
    }
}
