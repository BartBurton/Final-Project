using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoreUI : MonoBehaviour
{
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
            if (_selectedCharacterContainer.transform.childCount > 0)
            {
                Destroy(_selectedCharacterContainer.transform.GetChild(0).gameObject);
            }

            var gayo = Instantiate(selectedCharacter.CharacterPrefab);
            gayo.SetActive(true);
            gayo.transform.SetParent(_selectedCharacterContainer.transform, false);
        }
    }

    public void Back()
    {
        Loader.Load(Loader.Scene.MainMenu, false, false);
    }

    private void FixedUpdate()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
