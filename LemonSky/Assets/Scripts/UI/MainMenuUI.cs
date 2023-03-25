using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;

    void Awake(){
        playButton.onClick.AddListener(()=>{
            Loader.Load(Loader.Scene.Game);
        });
        quitButton.onClick.AddListener(()=>{
            Application.Quit();
        });
    }
}
