using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class placeObj : NetworkBehaviour,SpawnableObjParent
{
    [SerializeField] private GameObject spawnObject;
    public NetworkObject getNetwrokObject()
    {
        return NetworkObject;
    }

    public pickableObject GetObject()
    {
        return spawnObject.GetComponent<pickableObject>();
    }

    public Transform getObjectFollowTransform()
    {
        return transform;
    }

    public bool hasSpawnObject()
    {
        return spawnObject != null;
    }

    public bool IsListEmpty()
    {
        throw new System.NotImplementedException();
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
