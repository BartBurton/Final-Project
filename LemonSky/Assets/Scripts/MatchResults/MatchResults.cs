using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionResultItem
{
    public Guid PlayerId { get; set; }

    public int Rank { get; set; }
    public string Name { get; set; }
    public double Coins { get; set; }
    public int Punches { get; set; }
    public double Exp { get; set; }
    public int Fails { get; set; }
}

public class MatchResults : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerResultsText;
    [SerializeField] private TextMeshProUGUI _emptyListText;
    [SerializeField] private Button _toMenuButton;

    public Guid SessionId;
    public IEnumerable<SessionResultItem> SessionResultItems;

    private void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    async void Start()
    {
        _toMenuButton.onClick.AddListener(ToMenu);

        Guid sessionId = Guid.Empty;

        try
        {
            sessionId = (await APIRequests.LastSession()).Id;
        }
        catch { }

        if (sessionId != Guid.Empty)
        {
            try
            {
                var rtis = await APIRequests.RatingTable(Guid.Parse("9eabd9c9-4d0c-4a01-e6b6-08db5d0fa566"));

                SessionResultItems = rtis.Select(rti =>
                    new SessionResultItem()
                    {
                        PlayerId = rti.Account.Id,
                        Name = rti.Account.Name,
                        Rank = rti.Rank,
                        Coins = rti.Coins,
                        Punches = rti.Punches,
                        Exp = rti.Exp,
                        Fails = rti.Fails,
                    }
                );
            }
            catch { }
        }

        if (SessionResultItems != null && SessionResultItems.Count() != 0)
        {
            if (User.Id == Guid.Empty)
            {
                try
                {
                    User.SetUser(await APIRequests.WhoIAm());
                }
                catch { }
            }

            FillResults(SessionResultItems);
        }
        else
        {
            _emptyListText.gameObject.SetActive(true);
        }
    }

    private void FillResults(IEnumerable<SessionResultItem> results)
    {
        var sortedResults = results.OrderBy(i => i.Rank);

        foreach (var item in sortedResults)
        {
            ResultItemsContainer.Instance.AddResultItem(item);
        }

        if (User.Id != null)
        {
            var player = sortedResults.FirstOrDefault(i => i.PlayerId == User.Id);
            if (player != null)
            {
                _playerResultsText.text = $"Вы заняли {player.Rank} место";
            }
        }
    }

    private void ToMenu()
    {
        AudioShot.Instance.PlaySafely("main");
        Loader.Load(Loader.Scene.MainMenu, false, true);
    }
}
