using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class placeObj : NetworkBehaviour,SpawnableObjParent
{
    [SerializeField] private GameObject spawnObject;
    [SerializeField] private Transform placement;
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

    public bool hasSpawnObject()
    {
        return spawnObject != null;
    }

    public void setspawnObject(GameObject obj)
    {
        spawnObject = obj;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
