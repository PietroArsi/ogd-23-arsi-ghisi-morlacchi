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
    //public GameObject ground;


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
            //ground = GameObject.Find("map");
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
       // bool notPlacable = Physics.BoxCast(interactionCollider.transform.position, interactionCollider.localScale, Vector3.forward, Quaternion.identity, 1f);
        //Debug.Log("CAN I PLACE OBJECT" + notPlacable);
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
        Debug.Log("<color=yellow>PlayerNetwork: remove spawnObject</color>");
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
                Debug.Log("<color=yellow>PlayerNetwork: PickUp Object</color>");
            }
            else if(c.gameObject.GetComponent<GetWall>()) {

                   c.GetComponent<GetWall>().getWall();
                }
        }

    }
    private void PlaceDownObject()
    {
        Debug.Log("<color=yellow>PlayerNetwork Leave Object </color>");
       
        Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders)
        {
            // if (c.name == "Cube" && hasSpawnObject())
            // {
            //     GetObject().setObjectParent(c.GetComponent<SpawnableObjParent>());
            //     ClearSpawnObject();
            // }
            if (c.gameObject.GetComponent<SpawnableObjParent>() != null && hasSpawnObject() && !c.gameObject.GetComponent<SpawnableObjParent>().hasSpawnObject())
            {
                if (GetObject().transform.gameObject.name != "block1(Clone)")
                {
                    GetObject().setObjectParent(c.GetComponent<SpawnableObjParent>());
                    ClearSpawnObject();
                }
            }

        }
        //spawnObject.GetComponent<pickableObject>().setObjectParent(ground.GetComponent<SpawnableObjParent>());
        
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
      if(clientID == OwnerClientId && hasSpawnObject())
        {
            Destroy(spawnObject);
        }
    }

}
