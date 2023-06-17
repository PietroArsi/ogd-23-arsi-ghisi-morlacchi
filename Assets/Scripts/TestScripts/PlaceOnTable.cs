using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlaceOnTable : NetworkBehaviour,SpawnableObjParent
{
    //name PlaceOnTable;
    [SerializeField] private GameObject spawnObject;
    //[SerializeField] private Transform placement;
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

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public PickableObject GetObject()
    {
        return spawnObject.GetComponent<PickableObject>();
    }

    public Transform GetObjectFollowTransform()
    {
         return transform;
    }

    public int GetPriority()
    {
        return priority;
    }

    public bool HasSpawnObject()
    {
        return spawnObject != null;
    }

    public void SetspawnObject(GameObject obj)
    {
        spawnObject = obj;

        if (spawnObject != null)
        {
            OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
        }
    }

  
}
