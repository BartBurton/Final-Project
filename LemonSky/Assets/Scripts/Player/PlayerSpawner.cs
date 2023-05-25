using System.Linq;
using StarterAssets;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    [SerializeField] List<SpawnZone> spawnZones;
    public static PlayerSpawner Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    [ClientRpc]
    public void SpawnPlayerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Debug.Log("Запросил создать плеера - " + clientRpcParams.Receive);

        SpawnPlayerServerRpc((int)PlayerInitializer.Instance.GetSafePlayerType(CharacterSelector.Instance?.SelectedPlayer));
    }

    public Vector3 NextPosition()
    {
        var a = spawnZones[new System.Random().Next(spawnZones.Count)].NextRandomPosition();
        Debug.Log($"Следующая позиция - {a}");
        return a;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnPlayerServerRpc(int playerType, ServerRpcParams serverRpcParams = default)
    {
        Debug.Log("Все id - " + string.Join(", ", NetworkManager.ConnectedClients.Select(e => e.Key.ToString())));

        var clientId = serverRpcParams.Receive.SenderClientId;

        if (NetworkManager.ConnectedClients.ContainsKey(clientId))
        {
            Debug.Log($"Spawn ID {clientId}");

            var gayObject = Instantiate(
                PlayerInitializer.Instance.GetBasePlayerPrefab(),
                NextPosition(),
                Quaternion.identity
            );
            gayObject.GetComponent<ThirdPersonController>().SkinType.Value = playerType;
            gayObject.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }
}
