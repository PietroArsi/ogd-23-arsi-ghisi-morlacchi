using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GroundPlacement : NetworkBehaviour,SpawnableObjParent
{
    private int priority;
    //[SerializeField] private List<GameObject> placedObjectList = new List<GameObject>();
    private void Start()
    {
        priority = 0;
    }
    public void ClearSpawnObject()
    {
        return;
    }
    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public PickableObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetObjectFollowTransform()
    {
        return transform;
    }

    public bool HasSpawnObject()
    {
       return false;
    }

    public void SetspawnObject(GameObject obj)
    {
        return;
    }

    public int GetPriority()
    {
        return priority;
    }
}
