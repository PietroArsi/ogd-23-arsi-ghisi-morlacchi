using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyHoldCatnip : NetworkBehaviour, SpawnableObjParent, EnemyInteractable
{
    [SerializeField] private Transform holdPosition;
    [SerializeField] private GameObject spawnObject;
    private int priority;
    public AudioClip killEnemySound;

    void Start()
    {
        priority = 1;
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
        return priority;
    }

    public bool HasSpawnObject()
    {
        return spawnObject != null;
    }

    public void SetspawnObject(GameObject obj)
    {
        spawnObject = obj;
    }

    
    public void KillEnemy()
    {
        //PlayerGetCatnip(player);
        DestoryEnemyServerRpc();
    }
    public void DestroyCatnipStolen()
    {
        DestroyHeldObjectServerRpc(GetObject().gameObject);
    }
    [ServerRpc]
    private void DestroyHeldObjectServerRpc(NetworkObjectReference heldOject) {
        GameObject destoryObject = heldOject;
        if (destoryObject != null) {
            Destroy(destoryObject);
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DestoryEnemyServerRpc()
    {
       // GameObject spawnedPlace = gameObject.GetComponent<PawliceMovement>().GetSpawnMarker();
        visualDebugger.AddMessage("KILL ENEMY");
        // Destroy(spawnedPlace);
        // Destroy(gameObject);
        DestroyEnemyClientRpc();
    }

    [ClientRpc]
    private void DestroyEnemyClientRpc()
    {
        //GameObject spawnedPlace = gameObject.GetComponent<PawliceMovement>().GetSpawnMarker();
        //Destroy(spawnedPlace);
        if (killEnemySound != null)
        {
            GameObject.Find("UI sounds").transform.GetComponent<UISoundManager>().ClickSound(killEnemySound);
        }
        gameObject.GetComponent<PawliceMovement>().DestroyDogHome();
        Destroy(gameObject);
    }

}
