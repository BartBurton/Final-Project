using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    void Awake(){
        playButton.onClick.AddListener(()=>{
            SceneManager.LoadScene(1);
        });
        quitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
