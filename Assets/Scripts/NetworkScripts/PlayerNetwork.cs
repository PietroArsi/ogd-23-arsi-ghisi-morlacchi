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

    // functionality needed to place/move the picked object
    public Transform getObjectFollowTransform()
    {
        return holdPosition;
    }

    public bool hasSpawnObject()
    {
        return spawnObject != null;
    }

    public NetworkObject getNetworkObject()
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
        public void ClearSpawnObject()
    {
        spawnObject = null;
    }

    //this need to be updated to check when there is a big block i cannot place it on top of it
    private void GetInput()
    {
        if (Input.GetButtonDown("Fire1") && !hasSpawnObject())
            PickUpObject();
        if (Input.GetKeyDown(KeyCode.Space) && hasSpawnObject())
            PlaceDownObject();
    }

    private void PickUpObject()
    {
        
        Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders)
        {

            if (c.gameObject.GetComponent<pickableObject>())
            {
                c.gameObject.GetComponent<pickableObject>().currentParent().ClearSpawnObject();
                c.GetComponent<pickableObject>().setObjectParent(this);
                Debug.Log("<color=yellow>PickUp Object</color>");
            }
            else if(c.gameObject.GetComponent<GetWall>()) {

                   c.GetComponent<GetWall>().getWall();
                }
        }

    }
    private void PlaceDownObject()
    {
        Debug.Log("<color=yellow>Leave Object </color>");
        spawnObject.GetComponent<pickableObject>().setObjectParent(ground.GetComponent<SpawnableObjParent>());
        //spawnObject = null;
        Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders)
        {

            if (c.gameObject.GetComponent<SpawnableObjParent>() != null)
            {
                spawnObject.GetComponent<pickableObject>().setObjectParent(c.GetComponent<SpawnableObjParent>());

            }
        }
        //spawnObject.GetComponent<pickableObject>().setObjectParent(ground.GetComponent<SpawnableObjParent>());
        ClearSpawnObject();
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
      if(clientID == OwnerClientId && hasSpawnObject())
        {
            Destroy(spawnObject);
        }
    }

}
