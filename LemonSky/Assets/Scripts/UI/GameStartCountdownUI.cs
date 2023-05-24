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
        LocalUIManager.Instance.OnStateChanged += LocalUI_OnStateChanged;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        int countdownNumber = Mathf.CeilToInt(GameManager.Instance.GetCountdownToStartTimer());
        countdownText.text = countdownNumber.ToString();
        if (previousCountdownNumber != countdownNumber)
        {
            previousCountdownNumber = countdownNumber;
            AudioShot.Instance.Play("count-down");
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

    void LocalUI_OnStateChanged(LocalUIManager.UIState prev, LocalUIManager.UIState next)
    {
        if (next == LocalUIManager.UIState.CountDownToStart)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }
}
