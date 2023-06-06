using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

//Luca
public class PlayerNetwork : NetworkBehaviour,SpawnableObjParent
{
    public static event EventHandler OnAnyPlayerSpawned;
    public Transform holdPosition;
    public GameObject spawnObject;
    public GameObject temp;
    public bool collect;
    public GameObject ground;


    public LayerMask interactionLayer;
    public Transform interactionCollider;
    public static PlayerNetwork LocalIstance { get; private set; }

    [SerializeField] private PlayerVisual playerVisual;

    //private bool _carriedObject=false;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalIstance = this;
            this.transform.position = new Vector3(0, 0.61f, 0);
            ground = GameObject.Find("map");
            // temp = this.gameObject;
        }
        OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientDisconnectCallback;


      
    }
}
    public void Start()
    {

        PlayerData playerdData = ConnectionManager.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(ConnectionManager.Instance.GetPlayerColor(playerdData.colorId));
    }
    public void Update()
    {
        if (!IsOwner) return;
        GetInput();
    }
    public Transform getObjectFollowTransform()
    {
        return holdPosition;
    }

    public bool hasSpawnObject()
    {
        return spawnObject != null;
    }

    public NetworkObject getNetwrokObject()
    {
        return NetworkObject;
    }

    public pickableObject GetObject()
    {
        return spawnObject.GetComponent<pickableObject>();
    }

    public void setspawnObject(GameObject obj)
    {
        spawnObject = obj;
    }
    //this need to be updated to check when there is a big block i cannot place it on top of it
    private void GetInput()
    {


        collect = Input.GetKeyDown(KeyCode.Space);

        // need to modify here avoid bugs the probelm is when i find another interactable without the spawnableobjectParent; add a layer
        if (collect && hasSpawnObject())
        {
            spawnObject.GetComponent<pickableObject>().setObjectParent(ground.GetComponent<SpawnableObjParent>());
            //spawnObject = null;
            Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);
            foreach (Collider c in hitColliders)
            {

                if (c.gameObject.GetComponent<SpawnableObjParent>()!=null)
                {
                    spawnObject.GetComponent<pickableObject>().setObjectParent(c.GetComponent<SpawnableObjParent>());
                   
                }
            }
            //spawnObject.GetComponent<pickableObject>().setObjectParent(ground.GetComponent<SpawnableObjParent>());
            spawnObject = null;

        }

    }

    public bool IsListEmpty()
    {
        throw new NotImplementedException();
    }







    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
      if(clientID == OwnerClientId && hasSpawnObject())
        {
            spawnObject.GetComponent<pickableObject>().destoryObjec();
        }
    }

}
