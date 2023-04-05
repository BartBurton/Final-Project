using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] Button readyButton;

    void Awake()
    {
        readyButton.interactable = false;
        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
            readyButton.gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "OK";
            SelectCharacterManager.Instance.OnPlayerTypeChange -= HandlePlayerTypeChange;
        });
    }

    private void Start()
    {
        SelectCharacterManager.Instance.OnPlayerTypeChange += HandlePlayerTypeChange;
    }

    private void HandlePlayerTypeChange()
    {
        if (SelectCharacterManager.Instance.SelectedPlayer != null)
        {
            readyButton.interactable = true;
        } else
        {
            readyButton.interactable = false;
        }
    }
}
