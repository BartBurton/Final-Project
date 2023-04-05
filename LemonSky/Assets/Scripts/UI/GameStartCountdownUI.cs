using UnityEngine;
using TMPro;
using Unity.Netcode;

public class GameStartCountdownUI : MonoBehaviour
{
    const string Number_Popup = "NumberPopup";
    [SerializeField] TextMeshProUGUI countdownText;
    Animator animator;
    int previousCountdownNumber;
    void Start()
    {
        Hide();
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            if (GameManager.Instance.IsCountDownToStartActive())
                Show();
            else
                Hide();
        };
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void GameManager_OnStateChanged(object sender, System.EventArgs e)
    {
        if (GameManager.Instance.IsCountDownToStartActive())
            Show();
        else
            Hide();
    }

    void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            animator.SetTrigger(Number_Popup);
        }
    }

    void Show()
    {
        gameObject.SetActive(true);
    }
    void Hide()
    {
        gameObject.SetActive(false);
    }
}
