using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PlayStatisticManager : NetworkBehaviour
{
    public event Action<int> OnCoinCollected;
    public static PlayStatisticManager Instance { get; private set; }
    NetworkList<PlayerStatInfo> PlayersStatisticList;

    void Awake()
    {
        Instance = this;
        PlayersStatisticList = new();
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        if (!IsServer) return;
        GameManager.Instance.OnStateChanged += CreateList;
    }
    public override void OnDestroy()
    {
        PlayersStatisticList.Dispose();
        base.OnDestroy();
    }
    public int GetCollectedCoins(ulong clientId)
    {
        var index = IndexOfPlayer(clientId);
        if (index == -1) return 0;
        return PlayersStatisticList[index].CoinCount;
    }
    [ClientRpc]
    public void CoinCollectedClientRpc(int coinsCount, ClientRpcParams clientRpcParams = default)
    {
        OnCoinCollected?.Invoke(coinsCount);
    }
    int IndexOfPlayer(ulong clientId)
    {
        for (int i = 0; i < PlayersStatisticList.Count; i++)
            if (PlayersStatisticList[i].ClientId == clientId)
                return i;
        return -1;
    }
    void CreateList(object sender, EventArgs arg)
    {
        if (GameManager.Instance.IsCountDownToStartActive())
        {
            PlayersStatisticList.Clear();
            GetClientInfoClientRpc();
        }
    }

    [ClientRpc]
    void GetClientInfoClientRpc(ClientRpcParams clientRpcParams = default)
    {
        var info = new PlayerStatInfo()
        {
            Email = User.Email,
        };
        AddClientInfoServerRpc(info);
    }

    [ServerRpc(RequireOwnership = false)]
    void AddClientInfoServerRpc(PlayerStatInfo playerInfo, ServerRpcParams serverRpcParams = default)
    {
        try
        {
            var b = new PlayerStatInfo()
            {
                ClientId = serverRpcParams.Receive.SenderClientId,
                CoinCount = 0,
                Email = playerInfo.Email,
                Fails = 0,
                Punches = 0

            };
            PlayersStatisticList.Add(b);
        }
        catch (Exception e )
        {
            Debug.Log(e.ToString());
            throw;
        }
        Debug.Log("Создан -" + $"({serverRpcParams.Receive.SenderClientId})" + playerInfo.Email);
    }

    [ServerRpc(RequireOwnership = false)]
    void UpdateClientStatisticServerRpc(PlayerStatInfo playerInfo, ServerRpcParams serverRpcParams = default)
    {
        UpdateClientStatistic(playerInfo, serverRpcParams.Receive.SenderClientId);
    }
    void UpdateClientStatistic(PlayerStatInfo playerInfo, ulong clientId)
    {
        var index = IndexOfPlayer(clientId);
        if (index == -1) return;
        PlayersStatisticList[index] += playerInfo;
        Debug.Log("Обновлен -" + $"({clientId})" + playerInfo.Email);
    }



    public void Coin(ulong clientId)
    {
        var playerStat = new PlayerStatInfo()
        {
            ClientId = clientId,
            Email = User.Email,
            CoinCount = 1,
        };
        UpdateClientStatistic(playerStat, clientId);

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { clientId }
            }
        };
        CoinCollectedClientRpc(PlayersStatisticList[IndexOfPlayer(clientId)].CoinCount, clientRpcParams);
    }
    public void Fail(ulong clientId)
    {
        var playerStat = new PlayerStatInfo()
        {
            ClientId = clientId,
            Email = User.Email,
            Fails = 1,
        };
        UpdateClientStatistic(playerStat, clientId);
    }
    public void Punch(ulong clientId)
    {
        var playerStat = new PlayerStatInfo()
        {
            ClientId = clientId,
            Email = User.Email,
            Punches = 1,
        };
        UpdateClientStatistic(playerStat, clientId);
    }
    public void Dead(ulong clientId)
    {
        var playerStat = new PlayerStatInfo()
        {
            ClientId = clientId,
            Email = User.Email,
            DeathTime = DateTime.Now,
        };
        UpdateClientStatistic(playerStat, clientId);
    }
}
public struct PlayerStatInfo : INetworkSerializable, System.IEquatable<PlayerStatInfo>
{
    public ulong ClientId;
    public NetworkString Email;
    public int CoinCount;
    public int Fails;
    public int Punches;
    public DateTime DeathTime;
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        if (serializer.IsReader)
        {
            serializer.SerializeValue(ref Email);
            var reader = serializer.GetFastBufferReader();
            reader.ReadValueSafe(out ClientId);
            reader.ReadValueSafe(out CoinCount);
            reader.ReadValueSafe(out Fails);
            reader.ReadValueSafe(out Punches);
            reader.ReadValueSafe(out DeathTime);
        }
        else
        {
            // The complex type handles its own serialization
            serializer.SerializeValue(ref Email);
            // Now serialize the non-complex type properties
            var writer = serializer.GetFastBufferWriter();
            writer.WriteValueSafe(ClientId);
            writer.WriteValueSafe(CoinCount);
            writer.WriteValueSafe(Fails);
            writer.WriteValueSafe(Punches);
            writer.WriteValueSafe(DeathTime);
        }
    }
    public bool Equals(PlayerStatInfo other)
    {
        return other.Equals(this) && Email.ToString() == other.Email.ToString();
    }
    public override string ToString()
    {
        return $"ClientId - {ClientId}  CoinCount - {CoinCount}  Email - {Email}";
    }
    public static PlayerStatInfo operator +(PlayerStatInfo a, PlayerStatInfo b)
    {
        return new PlayerStatInfo()
        {
            ClientId = a.ClientId,
            Email = a.Email,
            CoinCount = a.CoinCount + b.CoinCount,
            Fails = a.Fails + b.Fails,
            Punches = a.Punches + b.Punches,
            DeathTime = b.DeathTime,
        };
    }
}

