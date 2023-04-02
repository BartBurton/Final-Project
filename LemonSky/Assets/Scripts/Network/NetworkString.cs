using System;
using Unity.Collections;
using Unity.Netcode;

public struct NetworkString : INetworkSerializable
{
    private ForceNetworkSerializeByMemcpy<FixedString128Bytes> data;

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref data);
    }

    public override string ToString() => data.Value.ToString();

    public static implicit operator string(NetworkString networkString) => networkString.ToString();
    public static implicit operator NetworkString(string s) => new NetworkString { data = new FixedString128Bytes(s) };
}