using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button createGameButton;
    [SerializeField] Button joinGameButton;

    void Awake(){
        createGameButton.onClick.AddListener(() => {GameMultiplayer.Instance.StartHost(); Loader.LoadNetwork(Loader.Scene.CharacterSelect); });
        joinGameButton.onClick.AddListener(() => {GameMultiplayer.Instance.StartClient();});
    }
}
