using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI resultText;
    [SerializeField] Button watchButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        watchButton.onClick.AddListener(() =>
        {
            GameInputs.Instance.IsPaused = false;
        });

        exitButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        CoinsManager.Instance.OnCoinCollected += CoinsManager_OnStateChanged;
    }

    void CoinsManager_OnStateChanged(int count)
    {
        resultText.text = count.ToString();
    }

    void Show()
    {
        Cursor.visible = true;
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
