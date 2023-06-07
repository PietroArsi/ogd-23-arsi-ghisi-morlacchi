using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using System;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

/// <summary>
/// Here are all the functionality of the lobby and the Relay and Authentichation services
/// </summary>
public class CatnipLobby : MonoBehaviour
{
    //use for the lobby data
    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
    public static CatnipLobby Instance { get; private set; }

    // Event to handle during the creation of the lobby
    public event EventHandler OnCreateLobbyStarted;
    public event EventHandler OnCreateLobbyFailed;

    //Event to hendle during client connection.
    public event EventHandler OnJoinStarted;
    public event EventHandler OnJoinFailed;

    //Event to get all the lobby available
    public event EventHandler <OnLobbyListChangedEventArgs> OnLobbyListChanged;
    public class OnLobbyListChangedEventArgs: EventArgs
    {
        public List<Lobby> lobbyList;
    }

    private Lobby joinedLobby;
    private float heartbeatTimer;
    private float lobbylistTimer;
    private void Awake()
    {
        if (Instance != null) Destroy(this.gameObject);
        Instance = this;
        DontDestroyOnLoad(gameObject);
        IntitializeUnityAuthentication();
    }

    //Use the anonymous authentication to sign in the player
    private async void IntitializeUnityAuthentication()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            InitializationOptions initalizationOptions = new InitializationOptions();
            initalizationOptions.SetProfile("Cat" + UnityEngine.Random.Range(0, 100).ToString());
           
            await UnityServices.InitializeAsync(initalizationOptions);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }

    }

    //allocation for the server Relay
    private async Task<Allocation> CreateAlllocationRelay()
    {
        try
        {

            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(ConnectionManager.MAX_NUMBER_PLAYER - 1); //region: "europe-central2"
            Debug.Log("<color=yellow>CatnipLobby Create Allocation</color>");

            return allocation;
        }catch(RelayServiceException e){

            Debug.Log(e);
            return default;
        }
    }

    //create the relay code form the allocation
    private async Task<string> GetRelayJoinCode(Allocation allocation)
    {
        try
        {
            string relayJoinCode=await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            return relayJoinCode;
        }
        catch (RelayServiceException e)
        {

            Debug.Log(e);
            return default;
        }
    }

    //Give the joincode to connet to the Host Relay
    private async Task<JoinAllocation> JoinRelay(string joinCode)
    {
        try
        {
           JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            return joinAllocation;
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
            return default;
        }
    }
    public async void CreateLobby(string lobbyName)
    {
        OnCreateLobbyStarted?.Invoke(this, EventArgs.Empty);
        try {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, ConnectionManager.MAX_NUMBER_PLAYER, new CreateLobbyOptions
            {

                IsPrivate = false,

            });

            /*
           Allocation allocation = await CreateAlllocationRelay();

           string relyaJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions
            {
                Data = new Dictionary<string, DataObject>
                {
                    { KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member,relyaJoinCode) }
                }
            });
           NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation,"dtls"));
           Debug.Log("<color=yellow>CatnipLobby ALLOCATION " + allocation.Region +"</color>");
            */
            Debug.Log("<color=yellow>CatnipLobby Put Relay in pause for testing</color>");
            ConnectionManager.Instance.StartHost();
            SceneLoader.LoadNetwork(SceneLoader.Scene.CharacterSlectionScreen);
            Debug.Log("<color=yellow>CatnipLobby: Created Lobby</color>");
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            OnCreateLobbyFailed?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Update()
    {
        HeandleHeartbeat();
        HandlePeriodicListLobbies();
    }
    private void HeandleHeartbeat()
    {
        if (isLobbyHost())
        {
            heartbeatTimer -= Time.deltaTime;
            if (heartbeatTimer <= 0f)
            {
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;
                Debug.Log("<color=yellow>KeepLobbyAlive</color>");
                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }
    private void HandlePeriodicListLobbies()
    {
        if (joinedLobby == null && AuthenticationService.Instance.IsSignedIn && SceneManager.GetActiveScene().name==SceneLoader.Scene.LobbyManagement.ToString())
        {
            //Debug.Log("<color=yellow>CatnipLobby See List</color>");
            lobbylistTimer -= Time.deltaTime;
            if (lobbylistTimer <= 0f)
            {
                float lobbylistTimerMax = 3f;
                lobbylistTimer = lobbylistTimerMax;
                
                ListLobbies();
            }
        }
    }

    private bool isLobbyHost()
    {
        return joinedLobby != null && joinedLobby.HostId== AuthenticationService.Instance.PlayerId;

    }
    public async void QuickJoin()
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            joinedLobby=await LobbyService.Instance.QuickJoinLobbyAsync();
            ConnectionManager.Instance.StartClient();
           // Debug.Log("HELLO CLIENT");
        }
        catch (LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }
        
    }

    public async void JoinwithId(string lobbyId)
    {
        OnJoinStarted?.Invoke(this, EventArgs.Empty);
        try
        {
            //when the client join the lobby.
            joinedLobby = await LobbyService.Instance.JoinLobbyByIdAsync(lobbyId);
            /*
            string relayJoinedCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;
            JoinAllocation joinAllocation = await JoinRelay(relayJoinedCode);
            Debug.Log("join " + joinAllocation.Region);
            Debug.Log(relayJoinedCode);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));
            */
            Debug.Log("<color=yellow>CatnipLobby: Put Relay in pause for testing</color>");
            ConnectionManager.Instance.StartClient();
            
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
            OnJoinFailed?.Invoke(this, EventArgs.Empty);
        }

    }
    // return the lobby information
    public Lobby GetLobby()
    {
        return joinedLobby;
    } 

    public async void DeleteLobby()
    {
        if (joinedLobby != null) {
            try {
                Debug.Log("<color=yellow>CatnipLobby: Delete Lobby</color>");
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);
                joinedLobby = null;
                } catch(LobbyServiceException e)
            {
                Debug.Log(e);
            }
        }
            
    }

    public async void LeaveLobby()
    {
        try
        {
            Debug.Log("<color=yellow>CatnipLobby: Leave Lobby</color>");
            await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

            joinedLobby = null;
        }catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }


    private async void ListLobbies()
    {
        try
        {
            QueryLobbiesOptions queryLobbiesOptions = new QueryLobbiesOptions
            {
                Filters = new List<QueryFilter>
            {
                new QueryFilter(QueryFilter.FieldOptions.AvailableSlots,"0", QueryFilter.OpOptions.GT)
            }
            };
            QueryResponse queryResponse = await LobbyService.Instance.QueryLobbiesAsync(queryLobbiesOptions);
            Debug.Log("<color=yellow>CatnipLobby Query Response Lobby: "+ queryResponse +"</color>");

           OnLobbyListChanged?.Invoke(this, new OnLobbyListChangedEventArgs
            {
                lobbyList = queryResponse.Results,
                
            });
           
        }
        catch(LobbyServiceException e)
        {
            Debug.Log(e);
        }
    }
}
