using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class DeathUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _resultText;
    [SerializeField] private Button _exitButton;

    private void Awake()
    {
        _exitButton.onClick.AddListener(() =>
        {
            AudioShot.Instance.PlaySafely("second");
            NetworkManager.Singleton.Shutdown();
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    void Start()
    {
        Hide();
        LocalUIManager.Instance.OnStateChanged += LocalUI_OnStateChanged;
        PlayStatisticManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }

    void LocalUI_OnStateChanged(LocalUIManager.UIState prev, LocalUIManager.UIState next)
    {
        if (next == LocalUIManager.UIState.Death)
        {
            AudioShot.Instance.Play("death");
            LocalAudioManager.Instance.StopMusic();
            LocalAudioManager.Instance.Stop3DSouds();
            Show();
        }
        else
        {
            Hide();
        }
    }

    void CoinsManager_OnStateChanged(int count)
    {
        _resultText.text = count.ToString();
    }
}
