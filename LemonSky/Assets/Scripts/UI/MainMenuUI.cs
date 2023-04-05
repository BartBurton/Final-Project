using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    void Awake(){
        playButton.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.Lobby, false, false);
        });
        quitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
