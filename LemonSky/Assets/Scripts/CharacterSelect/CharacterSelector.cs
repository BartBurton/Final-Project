using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public static CharacterSelector Instance { get; private set; }

    [SerializeField] private List<SelectCharacterItem> _characterItems;

    [SerializeField] private TextMeshProUGUI _selectedCharacterNameText;

    private PlayerType? _selectedPlayer = null;


    public event Action OnPlayerTypeChange;

    public PlayerType? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            _selectedPlayer = value;
            OnPlayerTypeChange?.Invoke();
        }
    }

    public string SlectedCharacterName { set => _selectedCharacterNameText.text = $"Теперь вы - {value}"; }

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        if (User.Stuffs == null)
        {
            try
            {
                User.SetUser(await APIRequests.WhoIAm());
            }
            catch { }
        }

        if (User.Stuffs != null)
        {
            bool firstSelected = false;

            foreach (var stuff in User.Stuffs)
            {
                var item = _characterItems.FirstOrDefault(ci => "Character_" + ci.TargetPlayerType.ToString() == stuff.GameKey);
                if (item != null && !item.gameObject.activeSelf)
                {
                    item.gameObject.SetActive(true);
                    item.SetName(stuff.Name);

                    OnPlayerTypeChange += item.HandlePlayerTypeChange;

                    if (!firstSelected)
                    {
                        SelectedPlayer = item.TargetPlayerType;
                        SlectedCharacterName = stuff.Name;
                        firstSelected = true;
                    }
                }
            }
        }
    }
}
