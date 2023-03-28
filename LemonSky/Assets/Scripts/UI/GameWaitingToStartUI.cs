using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWaitingToStartUI : MonoBehaviour
{
    void Start(){
        GameManager.Instance.OnLocalPlayerReadyChanged += GameManager_OnLocalPlayerChanged;
    }

    void Show(){
        gameObject.SetActive(true);
    }
    void Hide(){
        gameObject.SetActive(false);
    }
    void GameManager_OnLocalPlayerChanged(object sender, EventArgs e){
        if(GameManager.Instance.IsLocalPlayerReady()){
            Hide();
        }
    }
}
