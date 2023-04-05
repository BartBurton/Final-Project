using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button createGameButton;
    [SerializeField] Button joinGameButton;

    void Awake()
    {
        createGameButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            GameMultiplayer.Instance.StartHost();
#else
            GameMultiplayer.Instance.StartServer();
#endif
            Loader.LoadNetwork(Loader.Scene.CharacterSelect);
        });
        joinGameButton.onClick.AddListener(() => { GameMultiplayer.Instance.StartClient(); });
    }
}
