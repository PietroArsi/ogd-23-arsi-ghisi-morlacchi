using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;
using Unity.Services.Authentication;

/// <summary>
/// Script that handle the major functoinality of the network
/// This is a base for later development of the network funciotanluty i'm following the base and then 
/// when we have better knowledge of the scene and the workflow that we can use i'll adpat it
/// </summary>
public class ConnectionManager : NetworkBehaviour
{

    public static ConnectionManager Instance { get; private set; }


    [Header("Player Data")]
    // Create Player identification
    private NetworkList<PlayerData> playerDataNetworkList;
    private string playerName;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";
    public const int MAX_NUMBER_PLAYER = 4;
    [SerializeField] private List<Color> playerColorList;
    [SerializeField] private List<Material> playerMaterialList;
    //need to create when the game start
    //[SerializeField] private Transform carPrefab;

    [Header("Spawn Object")]
    //list of object that can spawn during gameplay
    [SerializeField] List<GameObject> spawnableObj;
    private GameObject mouseObj;
    private GameObject houseObj;




    //the different event tied with the player client
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailToJoinGame;

   

    //tell when the  netowkr list changes when player connects or leaves
   public event EventHandler onListPlayerDataChanged;

    private bool changeColorResult;

     //[SerializeField] private bool isHostScreen = false;
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(this);

        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(0, 100));
        playerDataNetworkList = new NetworkList<PlayerData>();

        playerDataNetworkList.OnListChanged += Data_onListPlayerDataChanged;
    }
    // Event that checks if a new player is connected
    private void Data_onListPlayerDataChanged(NetworkListEvent<PlayerData> changeEvent)
    {
     onListPlayerDataChanged?.Invoke(this, EventArgs.Empty);
    }

  ///---------------HANDLE PLAYER AND CONNECTIONS -----------------------------------///
    //create a player name
    public string GetPlayerName()
    {
        return playerName;
    }
    //get color of the Player
    public Material GetPlayerColor(int colorId)
    {
        Debug.Log("<color=yellow>ConnectionManager: Player color id: " + colorId +"</color>");
        return playerMaterialList[colorId];
    }
    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }
    // change the color
    public bool ChangePlayerColor(int colorId)
    {
        ChangePlayerColorServerRpc(colorId);
        return changeColorResult;
    }

    public void StartHost()
    {

        //NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallBack;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Server_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartHost();
        
        
    }
    //remove disconetted client
    private void NetworkManager_Server_OnClientDisconnectCallback(ulong clientId)
    {
       for(int i=0; i < playerDataNetworkList.Count; i++)
        {
            PlayerData playerData = playerDataNetworkList[i];
            if (playerData.clientID == clientId)
            {
                //disconetted
                playerDataNetworkList.RemoveAt(i);
            }
        }
    }

    //give data to conneted clients
    private void NetworkManager_OnClientConnectedCallback(ulong clientID)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientID = clientID,
            colorId = GetFirstAvailableColorId(),

        });
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    //fucntion called by the client in the lobby 
    private void NetworkManager_ConnectionApprovalCallBack(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        connectionApprovalResponse.Approved = true;
        // in the case the game already started
        if (SceneManager.GetActiveScene().name != SceneLoader.Scene.CharacterSlectionScreen.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game has already started";
            return;
        }
        //in case the lobby is full
        if(NetworkManager.Singleton.ConnectedClientsIds.Count>= MAX_NUMBER_PLAYER)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full";
            return;
        }
        connectionApprovalResponse.Approved = true;
        
    }



    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);

        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnetedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_Client_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    //when player connects
    private void NetworkManager_Client_OnClientConnetedCallback(ulong clientId)
    {
        SetPlayerNameServerRpc(GetPlayerName());
        SetPlayerIdServerRpc(AuthenticationService.Instance.PlayerId);
    }

    //------------------------- PLAYER SERVER RPC -------------------------//
    //client tell the server its name 
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerNameServerRpc(string playerName, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerName = playerName;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerIdServerRpc(string playerId, ServerRpcParams serverRpcParams = default)
    {
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.playerId = playerId;
        playerDataNetworkList[playerDataIndex] = playerData;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ChangePlayerColorServerRpc(int colorId, ServerRpcParams serverRpcParams = default)
    {
        //var clientId = serverRpcParams.Receive.SenderClientId;
        if (!IsColorAvailable(colorId))
        {
            //colorNotAvailable
          //  changeColorResult = false;
           // ChangeColorFalseClientRpc(clientId);
            return;
        }
        int playerDataIndex = getPlayerDataIndexFromClientId(serverRpcParams.Receive.SenderClientId);
        PlayerData playerData = playerDataNetworkList[playerDataIndex];

        playerData.colorId = colorId;
        playerDataNetworkList[playerDataIndex] = playerData;
        changeColorResult = true;
        //ChangeColorTrueClientRpc(clientId);
        return;
    }
    //[ClientRpc]
    //private void ChangeColorFalseClientRpc(ulong clientid, ClientRpcParams clientRpcParams = default)
    //{
    //    if (OwnerClientId == clientid) {
    //        changeColorResult = false;
    //        }

    //    // Run your client-side logic here!!
       
    //}
    //[ClientRpc]
    //private void ChangeColorTrueClientRpc(ulong clientid, ClientRpcParams clientRpcParams = default)
    //{
    //    if (OwnerClientId == clientid)
    //    {
    //        changeColorResult = true;
    //    }

    //    // Run your client-side logic here!!

    //}

    //Get the id function form the sender of the ServerRpc
    private int getPlayerDataIndexFromClientId(ulong senderClientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientID == senderClientId)
            {
                return i;
            }
        }
        return -1;
    }
    //check if the color for character selection is available
    private bool IsColorAvailable(int colorId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
        {
            if (playerData.colorId == colorId)
            {
                //already used
                return false;
            }
        }
        return true;
    }
    //get the first color that is not used
    private int GetFirstAvailableColorId()
    {
        for (int i = 0; i < playerMaterialList.Count; i++)
        {
            if (IsColorAvailable(i))
            {
                return i;
            }
        }
        return -1;
    }
    //get list Color
    public List<Material> totalColors()
    {
        return playerMaterialList;
    }

    // On player Disconnet 
    private void NetworkManager_Client_OnClientDisconnectCallback(ulong clientID)
    {
        if (!IsHost)
        {
            OnFailToJoinGame?.Invoke(this, EventArgs.Empty);
        }
    }

    //check if the specific player index is connected
    public bool IsPlayerIndexConnected(int playerIndex)
    {
        if (playerIndex < playerDataNetworkList.Count)
        {
            return true;
        }
        else
            return false;
    }
    // Getting information from the player data
    public PlayerData GetPlayerDataFromPlayerIndex(int playerIndex)
    {
        return playerDataNetworkList[playerIndex];
    }
    public PlayerData GetPlayerDataFromClientId(ulong clientId)
    {
        foreach (PlayerData playerData in playerDataNetworkList)
            if (playerData.clientID == clientId)
                return playerData;
        return default;
    }
    public int GetPlayerDataIndexFromClientId(ulong clientId)
    {
        for (int i = 0; i < playerDataNetworkList.Count; i++)
        {
            if (playerDataNetworkList[i].clientID == clientId)
            {
                Debug.Log(i);
                return i;
            }
        }
        return -1;
    }
    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }


    public bool showYou(ulong clientId)
    {
        PlayerData playerData = GetPlayerData();
        if (playerData.clientID == clientId)
            return true;
        else
            return false;

    }


    //------------------------- HANDLE THE SPAWNABLE OBJECTS------------------------------------------////

    //Functionality to spawn an non player object 
    public void spawnNetworkObject(GameObject currentObj, SpawnableObjParent parent)
    {
        //Debug.Log(currentObj);
        SpawnObjServerRpc(GetSpawnIndex(currentObj),parent.GetNetworkObject());
    }

    public void SpawnEnemyDog(Vector3 spawnPosition,GameObject enemy)
    {
        SpawnEnemyDogServerRpc(GetSpawnIndex(enemy), spawnPosition);
    }
    public void SpawnEnemyMouse(Vector3 spawnPosition, GameObject enemy)
    {
        SpawnEnemyMouseServerRpc(GetSpawnIndex(enemy), spawnPosition);
    }
    public void SpawnEnemyHouse(Vector3 spawnPosition, GameObject house)
    {
        SpawnEnemyHouseServerRpc(GetSpawnIndex(house), spawnPosition);
    }

    // spawn the object in the server.
    [ServerRpc(RequireOwnership = false)]
    private void SpawnObjServerRpc(int current, NetworkObjectReference currParent, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject currentObj = GetSpawnFromId(current);
        visualDebugger.AddMessage("Sender of message for spawn: "+ clientId);


        currentObj = Instantiate(currentObj, new Vector3(UnityEngine.Random.Range(0, 10), 1, UnityEngine.Random.Range(0, 10)), Quaternion.identity);
        NetworkObject objNetworkObject = currentObj.GetComponent<NetworkObject>();
        objNetworkObject.Spawn(true);

        PickableObject objectPicked = currentObj.GetComponent<PickableObject>();
        currParent.TryGet(out NetworkObject currentParentObject);
        SpawnableObjParent parent = currentParentObject.GetComponent<SpawnableObjParent>();
        //Debug.Log(currentParentObject.GetComponent<SpawnableObjParent>()==null);
        objectPicked.setObjectParent(parent);
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnEnemyDogServerRpc(int current,Vector3 spawnPosition, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject currentObj = GetSpawnFromId(current);
        visualDebugger.AddMessage("Sender of message for spawn: " + clientId);


        currentObj = Instantiate(currentObj, spawnPosition, transform.rotation);
        NetworkObject objNetworkObject = currentObj.GetComponent<NetworkObject>();
        objNetworkObject.Spawn(true);
    }
    [ServerRpc(RequireOwnership =false)]
    private void SpawnEnemyHouseServerRpc(int current, Vector3 spawnPosition, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject currentObj = GetSpawnFromId(current);
        //visualDebugger.AddMessage("Sender of message for spawn: " + clientId);


        currentObj = Instantiate(currentObj, spawnPosition, transform.rotation);
        NetworkObject objNetworkObject = currentObj.GetComponent<NetworkObject>();
        objNetworkObject.Spawn(true);
        SetHouseObj(currentObj);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnEnemyMouseServerRpc(int current, Vector3 spawnPosition, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject currentObj = GetSpawnFromId(current);
        visualDebugger.AddMessage("Sender of message for spawn: " + clientId);


        currentObj = Instantiate(currentObj, spawnPosition, Quaternion.identity);
        NetworkObject objNetworkObject = currentObj.GetComponent<NetworkObject>();
        objNetworkObject.Spawn(true);
        SetMouseObject(currentObj);
    }

    //to solve the problem of spawning objects
    private int GetSpawnIndex(GameObject current) 
    {
        //Debug.Log("NAME OBJECT "+current.name);
        return  spawnableObj.IndexOf(current);
    }
    public GameObject GetSpawnFromId(int id)
    {
        //Debug.Log(id);
        return spawnableObj[id];
    }

    private void SetMouseObject(GameObject mouse)
    {
        mouseObj = mouse;
    }
    public GameObject GetMouseObject()
    {
        return mouseObj;
    }
    private void SetHouseObj(GameObject house)
    {
        houseObj = house;
    }
    public GameObject GetEnemyHouse()
    {
        return houseObj;
    }
    //------------------------- HANDLE THE SPAWNABLE OBJECTS



    //Handle Host and join
}
