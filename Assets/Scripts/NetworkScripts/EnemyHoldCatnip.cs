using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHoldCatnip : NetworkBehaviour, SpawnableObjParent
{
    [SerializeField] private Transform holdPosition;
    [SerializeField] private GameObject spawnObject;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        return holdPosition;
    }

    public int GetPriority()
    {
        throw new System.NotImplementedException();
    }

    public bool HasSpawnObject()
    {
        return spawnObject != null;
    }

    public void SetspawnObject(GameObject obj)
    {
        spawnObject = obj;
    }

    public void PlaceCatnipGround()
    {

    }
    public void DestroyCatnipStolen()
    {
        DestroyHeldObjectServerRpc(GetObject().gameObject);
    }
    [ServerRpc]
    private void DestroyHeldObjectServerRpc(NetworkObjectReference heldOject)
    {
        GameObject destoryObject = heldOject;
        Destroy(destoryObject);
    }
}
