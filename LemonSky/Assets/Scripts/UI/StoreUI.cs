using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI _errorText;

    [Serializable]
    private class SelectibleCharacter
    {
        public PlayerType CharacterType;
        public GameObject CharacterPrefab;
    }
    public static StoreUI Instance { get; private set; }

    public Action UnselectPrev = () => { };

    [SerializeField] private List<SelectibleCharacter> _selectibleCharacters;

    [SerializeField] private GameObject _selectedCharacterContainer;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectChatacter(PlayerType playerType)
    {
        var selectedCharacter = _selectibleCharacters.FirstOrDefault(sc => sc.CharacterType == playerType);
        if (selectedCharacter != null)
        {
            UnSelectCharacter();

            var gayo = Instantiate(selectedCharacter.CharacterPrefab);
            gayo.SetActive(true);
            gayo.transform.SetParent(_selectedCharacterContainer.transform, false);
        }

        _errorText.gameObject.SetActive(false);
    }

    public void UnSelectCharacter()
    {
        if (_selectedCharacterContainer.transform.childCount > 0)
        {
            Destroy(_selectedCharacterContainer.transform.GetChild(0).gameObject);
        }
    }

    public void Back()
    {
        Loader.Load(Loader.Scene.MainMenu, false, false);
    }

    public void ActivateError()
    {
        _errorText.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
