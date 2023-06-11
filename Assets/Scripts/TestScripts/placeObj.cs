using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class placeObj : NetworkBehaviour,SpawnableObjParent
{
    //name placeTable;
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private Transform placement;
    private int priority;
    public static event EventHandler OnAnyObjectPlacedHere;

    void Start()
    {
        priority = 10;
    }
    public void ClearSpawnObject()
    {
        spawnObject = null;
    }

    public NetworkObject getNetworkObject()
    {
        return NetworkObject;
    }

    public pickableObject GetObject()
    {
        return spawnObject.GetComponent<pickableObject>();
    }

    public Transform getObjectFollowTransform()
    {
        if (placement != null)
            return placement;
       else
            return transform;
    }

    public int GetPriority()
    {
        return priority;
    }

    public bool hasSpawnObject()
    {
        return spawnObject != null;
    }

    public void setspawnObject(GameObject obj)
    {
        spawnObject = obj;

        if (spawnObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

  
}
