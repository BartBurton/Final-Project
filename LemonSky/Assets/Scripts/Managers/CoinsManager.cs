using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class CoinsManager : NetworkBehaviour
{
    public event Action<int> OnCoinCollected;
    public static CoinsManager Instance { get; private set; }
    NetworkList<PlayerCoinInfo> PlayersCoinsList;

    void Awake()
    {
        Instance = this;
        PlayersCoinsList = new();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
        GameManager.Instance.OnStateChanged += CreateList;
    }
    public override void OnDestroy()
    {
        PlayersCoinsList.Dispose();
        base.OnDestroy();
    }

    public int GetCollectedCoins(ulong clientId)
    {
        var index = IndexOfPlayerCoin(clientId);
        if (index == -1) return 0;
        return PlayersCoinsList[index].CoinCount;
    }
    public void CoinCollected(ulong clientId)
    {
        var index = IndexOfPlayerCoin(clientId);
        if (index == -1) return;
        var coinInfo = PlayersCoinsList[index];
        coinInfo.CoinCount++;
        PlayersCoinsList[index] = coinInfo;
        
        Debug.Log("Текущее состояние массива по монетам");
        for(int i = 0; i < PlayersCoinsList.Count; i++ ){
            Debug.Log(PlayersCoinsList[i].ToString());
        }

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        CoinCollectedClientRpc(PlayersCoinsList[index].CoinCount, clientRpcParams);
    }
    [ClientRpc]
    public void CoinCollectedClientRpc(int coinsCount, ClientRpcParams clientRpcParams = default)
    {
        OnCoinCollected?.Invoke(coinsCount);
    }
    int IndexOfPlayerCoin(ulong clientId)
    {
        for (int i = 0; i < PlayersCoinsList.Count; i++)
            if (PlayersCoinsList[i].ClientId == clientId)
                return i;
        return -1;
    }

    void CreateList(object sender, EventArgs arg)
    {
        if (GameManager.Instance.IsCountDownToStartActive())
            foreach (var client in NetworkManager.Singleton.ConnectedClients)
            {
                PlayersCoinsList.Add(new PlayerCoinInfo()
                {
                    ClientId = client.Value.ClientId,
                    CoinCount = 0,
                    Email = ""
                });
                Debug.Log("Создан -" + client.Value.ClientId);
            }
    }
    public struct PlayerCoinInfo : INetworkSerializable, System.IEquatable<PlayerCoinInfo>
    {
        public ulong ClientId;
        public int CoinCount;
        public NetworkString Email;
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            if (serializer.IsReader)
            {
                serializer.SerializeValue(ref Email);
                var reader = serializer.GetFastBufferReader();
                reader.ReadValueSafe(out ClientId);
                reader.ReadValueSafe(out CoinCount);
            }
            else
            {
                // The complex type handles its own serialization
                serializer.SerializeValue(ref Email);
                // Now serialize the non-complex type properties
                var writer = serializer.GetFastBufferWriter();
                writer.WriteValueSafe(ClientId);
                writer.WriteValueSafe(CoinCount);
            }
        }
        public bool Equals(PlayerCoinInfo other)
        {
            return other.Equals(this) && Email.ToString() == other.Email.ToString() && ClientId == other.ClientId;
        }
        public override string ToString()
        {
            return $"ClientId - {ClientId}  CoinCount - {CoinCount}  Email - {Email}";
        }
    }
}
