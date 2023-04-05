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
            Loader.Load(Loader.Scene.CharacterSelect, true, true);
        });
        joinGameButton.onClick.AddListener(() => { GameMultiplayer.Instance.StartClient(); });
    }
}
