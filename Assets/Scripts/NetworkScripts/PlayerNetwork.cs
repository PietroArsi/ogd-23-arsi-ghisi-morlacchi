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
    [SerializeField] private List<Vector3> spawnPositionList;
    //public GameObject ground;


    public LayerMask interactionLayer;
    public Transform interactionCollider;
    public static PlayerNetwork LocalIstance { get; private set; }

    [SerializeField] private PlayerVisual playerVisual;

    //private bool _carriedObject=false;
    // Start is called before the first frame update
    public void Start()
    {

        PlayerData playerdData = ConnectionManager.Instance.GetPlayerDataFromClientId(OwnerClientId);
        playerVisual.SetPlayerColor(ConnectionManager.Instance.GetPlayerColor(playerdData.colorId));
    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalIstance = this;
            transform.position = spawnPositionList[ConnectionManager.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
            //this.transform.position = new Vector3(0, 0.61f, 0);
            //ground = GameObject.Find("map");
            // temp = this.gameObject;
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        }
        
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientDisconnectCallback;
    }
}
   
    public void Update()
    {
        if (!IsOwner) return;
        GetInput();
       // bool notPlacable = Physics.BoxCast(interactionCollider.transform.position, interactionCollider.localScale, Vector3.forward, Quaternion.identity, 1f);
        //Debug.Log("CAN I PLACE OBJECT" + notPlacable);
    }

    // functionality needed to place/move the picked object
    public Transform GetObjectFollowTransform()
    {
        return holdPosition;
    }

    public bool HasSpawnObject()
    {
        return spawnObject != null;
    }

    public NetworkObject GetNetworkObject()
    {
        return NetworkObject;
    }

    public PickableObject GetObject()
    {
        return spawnObject.GetComponent<PickableObject>();
    }

    public void SetspawnObject(GameObject obj)
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
        if (Input.GetButtonDown("Fire1") && !HasSpawnObject())
        {
            // PickUpObject();
            interactionCollider.GetComponent<PickAndPlace>().PickUpObject(this);
        }
        if (Input.GetKeyDown(KeyCode.Space) && HasSpawnObject())
        {
            //PlaceDownObject();
            interactionCollider.GetComponent<PickAndPlace>().PlaceDownObject(this);
        }
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    {
    if(clientID == OwnerClientId && HasSpawnObject())
        {
            Destroy(spawnObject);
        }
    }

    public int GetPriority()
    {
        throw new NotImplementedException();
    }
}
