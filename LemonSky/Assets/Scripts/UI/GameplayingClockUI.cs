using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayingClockUI : MonoBehaviour
{
    [SerializeField] Image timerImage;

    void Update(){
        timerImage.fillAmount = GameManager.Instance.GetGameplayingTimerNormalize();
    }
}
