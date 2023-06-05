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

  

    // Create Player identification
    private NetworkList<PlayerData> playerDataNetworkList;
    private string playerName;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";
    public const int MAX_NUMBER_PLAYER = 4; 

    //list of object that can spawn during gameplay
    [SerializeField] List<GameObject> spawnableObj;
    
   
   

    //the different event tied with the player client
    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailToJoinGame;

    //need to create when the game start
    [SerializeField] private Transform playerPrefab;

    //tell when the  netowkr list changes when player connects or leaves
   public event EventHandler onListPlayerDataChanged;

   //  [SerializeField] private bool inGame = false;
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        Instance = this;
        
        Screen.SetResolution(1920, 1080, false);

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

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer) {
            //NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback; 
        }
    }


    //Spawn the players when we are in the game level.
    public void spawnPlayers()
    {
        if (IsServer)
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
    }
    
    
    //Load the player in the game.
    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
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

    private void NetworkManager_OnClientConnectedCallback(ulong clientID)
    {
        playerDataNetworkList.Add(new PlayerData
        {
            clientID = clientID,
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

    //get the corret element form the client that caalled the serverRpc.
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
    public PlayerData GetPlayerData()
    {
        return GetPlayerDataFromClientId(NetworkManager.Singleton.LocalClientId);
    }



    //------------------------- HANDLE THE SPAWNABLE OBJECTS------------------------------------------////

    //in the future when the player gonna carry like a block to place in the map
    public void spawnNetworkObject(GameObject currentObj, SpawnableObjParent parent)
    { 
        spawnObjServerRpc(getSpawnIndex(currentObj),parent.getNetwrokObject());
    }

   
    [ServerRpc(RequireOwnership = false)]
    private void spawnObjServerRpc(int current, NetworkObjectReference currParent, ServerRpcParams serverRpcParams = default)
    {
        var clientId = serverRpcParams.Receive.SenderClientId;
        GameObject currentObj = GetSpawnFromId(current);
        visualDebugger.AddMessage("Sender of message for spawn: "+ clientId);


        currentObj = Instantiate(currentObj, new Vector3(UnityEngine.Random.Range(0, 10), 1, UnityEngine.Random.Range(0, 10)), Quaternion.identity);
        NetworkObject objNetworkObject = currentObj.GetComponent<NetworkObject>();
        objNetworkObject.Spawn(true);

        pickableObject objectPicked = currentObj.GetComponent<pickableObject>();
        currParent.TryGet(out NetworkObject currentParentObject);
        SpawnableObjParent parent = currentParentObject.GetComponent<SpawnableObjParent>();
        objectPicked.setObjectParent(parent);
    }

    //to solve the problem of spawning objects
    private int getSpawnIndex(GameObject current) 
    {
        return  spawnableObj.IndexOf(current);
    }
    public GameObject GetSpawnFromId(int id)
    {
        return spawnableObj[id];
    }

    //------------------------- HANDLE THE SPAWNABLE OBJECTS
}
