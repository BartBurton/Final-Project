using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectUI : MonoBehaviour
{
    [SerializeField] private Button _readyButton;
    [SerializeField] private GameObject _waitPanel;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        _waitPanel.SetActive(false);

        _readyButton.onClick.AddListener(() =>
        {
            AudioShot.Instance.PlaySafely("main");
            CharacterSelectReady.Instance.SetPlayerReady();
            _waitPanel.SetActive(true);
            _readyButton.gameObject.SetActive(false);
        });
    }
}
