using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] Button readyButton;

    void Awake()
    {
        readyButton.interactable = false;
        readyButton.onClick.AddListener(() =>
        {
            CharacterSelectReady.Instance.SetPlayerReady();
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
