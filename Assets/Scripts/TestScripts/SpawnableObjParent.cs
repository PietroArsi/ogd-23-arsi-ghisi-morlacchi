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
    public PickableObject GetObject();
    public Transform GetObjectFollowTransform();
    public bool HasSpawnObject();
    public NetworkObject GetNetworkObject();
    public void SetspawnObject(GameObject obj);
    public void ClearSpawnObject();

    public int GetPriority();
    //public bool IsListEmpty();
}
