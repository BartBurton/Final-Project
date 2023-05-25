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
    [SerializeField] TextMeshProUGUI SearchersText;

    Guid _findedSession = Guid.Empty; IEnumerator coroutine;
    bool _isSearch = false;
    int _searchers = 1;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        SearchButton.gameObject.SetActive(true);
        StopSearchPanel.SetActive(false);
        SearchButton.onClick.AddListener(SearchClick);
        StopSearchButton.onClick.AddListener(StopSearch);
        errorText.enabled = false;
        GameMultiplayer.Instance.OnFailJoinGame += GameMultiplayer_OnFailJoinGame;
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
        AudioShot.Instance.PlaySafely("main");
        Loader.BeforeLoad += async () => { Store.Stuffs = await APIRequests.GetStuffs(); };
        Loader.Load(Loader.Scene.Store, false);
    }
    public void Quit()
    {
        AudioShot.Instance.PlaySafely("second");
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
        AudioShot.Instance.Play("main");

        InSearch();
        var map = (await APIRequests.GetMaps()).ElementAtOrDefault(0);
        if (map is null)
        {
            OutSearch();
            ShowError("Выбранная карта не найдена");
            return;
        }
        if (!_isSearch) return;
        await Task.Delay(new System.Random().Next(1000, 4000));
        if (!_isSearch) return;
        var session = await APIRequests.SearchSession(map.Id, 60);
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
            var sessionData = await APIRequests.StatusSession(_findedSession);
            session = sessionData.Session;
            _searchers = sessionData.Searchers;
            if (!_isSearch) return;
            Debug.Log("Сессия обновлена " + ++count + " раз");
            await Task.Delay(2000);
        }
        if (!_isSearch) return;
#warning Не реализовано подключение по session.GameKey
        Loader.BeforeLoad += async () => { GameMultiplayer.Instance.StartClient(); };
        Loader.Load(Loader.Scene.Loading);
    }
    async void StopSearch()
    {
        AudioShot.Instance.Play("second");

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
            SearchersText.text = _searchers.ToString();
            SearchTimer.text = $"{(int)(time / 60)}:{(int)(time % 60)}";
        }
    }

    void InSearch()
    {
        _isSearch = true;
        _searchers = 1;
        SearchButton.gameObject.SetActive(false);
        StopSearchPanel.SetActive(true);
        coroutine = StartSearchTimer();
        StartCoroutine(coroutine);
        Debug.Log("Начался поиск");
    }
    void OutSearch()
    {
        _isSearch = false;
        _searchers = 1;
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

    void OnApplicationQuit()
    {
        if (_isSearch) StopSearch();
    }
}
