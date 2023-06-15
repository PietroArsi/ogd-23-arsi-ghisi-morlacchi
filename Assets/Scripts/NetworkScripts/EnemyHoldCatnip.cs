using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHoldCatnip : NetworkBehaviour, SpawnableObjParent, EnemyInteractable
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

    private void PlayerGetCatnip(PlayerNetwork player)
    {
        if (HasSpawnObject()){
            GetObject().setObjectParent(player.GetComponent<SpawnableObjParent>());
        }
    }
    public void KillEnemy(PlayerNetwork player)
    {
        PlayerGetCatnip(player);
        DestoryEnemyServerRpc();
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


    [ServerRpc(RequireOwnership =false)]
    private void DestoryEnemyServerRpc()
    {
        GameObject spawnedPlace = gameObject.GetComponent<PawliceMovement>().GetSpawnMarker();
        visualDebugger.AddMessage("KILL ENEMY");
        Destroy(spawnedPlace);
        Destroy(gameObject);
    }

    
}
