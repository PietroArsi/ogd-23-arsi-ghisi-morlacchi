using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

//Luca
public class PlayerNetwork : NetworkBehaviour, SpawnableObjParent
{
    public static event EventHandler OnAnyPlayerSpawned;
    public Transform holdPosition;
    public GameObject spawnObject;
    public GameObject temp;
    public bool collect;

    public bool isPlayerCutting;

    //public GameObject ground;

    [SerializeField] private List<Vector3> spawnPositionList;
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
        isPlayerCutting = false;
    }
    public void SetPlayerPosition(Vector3 pos)
    {
        transform.position = pos;
    }
    public Vector3 GetPlayerPosition()
    {
        return transform.position;
    }
    //[ServerRpc(RequireOwnership =false)]
    //private void SetPlayerPositionServerRpc()
    //{
    //    transform.position = GetPlayerPosition();
    //    SetPlayerPositionClientRpc(transform.position);
    //}
    //[ClientRpc]
    //private void SetPlayerPositionClientRpc(Vector3 pos)
    //{
    //    transform.position = pos;

    //}
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            LocalIstance = this; 
        }
           // SetPlayerPositionServerRpc();
            //transform.position = GameManagerStates.Instance.spawnPositionList[ConnectionManager.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)].position;
           // transform.position = spawnPositionList[ConnectionManager.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)];
        //Debug.Log(GameManagerStates.Instance.spawnPositionList[ConnectionManager.Instance.GetPlayerDataIndexFromClientId(OwnerClientId)].position);
        //Debug.Log(transform.position);
        //this.transform.position = new Vector3(10, 2f, 5);
        //ground = GameObject.Find("map");
        // temp = this.gameObject;
        Debug.Log(transform.position);
            OnAnyPlayerSpawned?.Invoke(this, EventArgs.Empty);
        
        if (IsServer)
        {
            //NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientDisconnectCallback;
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
        if (Input.GetButtonDown("Fire1"))
        {
            if (!HasSpawnObject())
            {
                // PickUpObject();
                interactionCollider.GetComponent<PickAndPlace>().PickUpObject(this);
            }
            else if (HasSpawnObject())
            {
                // PickUpObject();
                interactionCollider.GetComponent<PickAndPlace>().PlaceDownObject(this);
            }
        }
        if (Input.GetKeyDown(KeyCode.E) && !HasSpawnObject())
        {
            interactionCollider.GetComponent<PickAndPlace>().CutCatnip(this);
        }
        //if (Input.GetKeyDown(KeyCode.Space) && HasSpawnObject())
        //{
        //    //PlaceDownObject();
        //    interactionCollider.GetComponent<PickAndPlace>().PlaceDownObject(this);
        //}
    }

    //private void NetworkManager_OnClientDisconnectCallback(ulong clientID)
    //{
    //    if(clientID == OwnerClientId && HasSpawnObject())
    //    {
    //        Destroy(spawnObject);
    //    }
    //}

    public int GetPriority()
    {
        throw new NotImplementedException();
    }
}
