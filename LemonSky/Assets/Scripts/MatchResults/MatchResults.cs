using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MatchResults : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerResultsText;
    [SerializeField] private TextMeshProUGUI _emptyListText;
    [SerializeField] private Button _toMenuButton;

    public static Guid SessionId;
    public static IEnumerable<SessionResultItem> SessionResultItems;

    async void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _toMenuButton.onClick.AddListener(ToMenu);

        if (SessionResultItems == null)
        {
            try
            {
                if (SessionId != null)
                {
                    SessionResultItems = await APIRequests.SessionResults(Guid.NewGuid());
                }
            }
            catch { }
        }

        if (SessionResultItems != null && SessionResultItems.Count() != 0)
        {
            if (User.Id == null)
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
