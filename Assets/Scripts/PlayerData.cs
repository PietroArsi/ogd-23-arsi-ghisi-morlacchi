using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Unity.Collections;

public struct PlayerData:IEquatable<PlayerData>, INetworkSerializable
{
    //here we store the player data needed for passing information in differnet networked scene
    public ulong clientID;
    public FixedString64Bytes playerName;
    public FixedString64Bytes playerId;

    public bool Equals(PlayerData other)
    {
        return clientID == other.clientID && playerName==other.playerName && playerId==other.playerId;
    }

    public string NameGenerator()
    {
        return ("Cat" + UnityEngine.Random.Range(0, 100).ToString());
    }
    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref clientID);
        serializer.SerializeValue(ref playerName);
        serializer.SerializeValue(ref playerId);
    }
}
