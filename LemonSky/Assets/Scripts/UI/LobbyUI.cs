using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button createGameButton;
    [SerializeField] Button joinGameButton;

    void Awake(){
        createGameButton.onClick.AddListener(() => {GameMultiplayer.Instance.StartServer(); Loader.Load(Loader.Scene.CharacterSelect, true, true); });
        joinGameButton.onClick.AddListener(() => {GameMultiplayer.Instance.StartClient();});
    }
}
