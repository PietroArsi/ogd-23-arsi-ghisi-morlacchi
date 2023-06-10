using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GroundPlacement : NetworkBehaviour,SpawnableObjParent
{
    //[SerializeField] private List<GameObject> placedObjectList = new List<GameObject>();

    public void ClearSpawnObject()
    {
        return;
    }
    public NetworkObject getNetworkObject()
    {
        return NetworkObject;
    }

    public pickableObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public Transform getObjectFollowTransform()
    {
        return transform;
    }

    public bool hasSpawnObject()
    {
       return false;
    }

    public void setspawnObject(GameObject obj)
    {
        return;
    }
}
