using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCharacterManager : MonoBehaviour
{
    public static SelectCharacterManager Instance { get; private set; }

    private PlayerType? _selectedPlayer = null;
    public PlayerType? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            _selectedPlayer = value;
            OnPlayerTypeChange?.Invoke();
        }
    }

    public event Action OnPlayerTypeChange;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
    }
}
