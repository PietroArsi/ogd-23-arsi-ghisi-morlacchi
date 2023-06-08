using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// this is a smaple functionality to set the parent of the object that has been picked up. 
/// the name can be changed, in our project if we want to build like a fortress.
/// </summary>
public interface SpawnableObjParent
{
    public pickableObject GetObject();
    public Transform getObjectFollowTransform();
    public bool hasSpawnObject();
    public NetworkObject getNetworkObject();
    public void setspawnObject(GameObject obj);

    //public bool IsListEmpty();
}
