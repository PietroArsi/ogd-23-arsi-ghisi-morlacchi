using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManagerStates : NetworkBehaviour
{
    public static GameManagerStates Instance { get; private set; }
    [SerializeField] private Transform playerPrefab;
    [SerializeField] private bool isConstructionWindowOpen;
    [SerializeField] public List<Transform> spawnPositionList;
    //varoious GameState;
    private enum State
    {
        WaitingOtherPlayers,
        CountdownToStart,
        GamePlaying,
        GameEnd,
        PlayerDisconnected,
    }
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;

    private bool isLocalPlayerReady;
    [SerializeField] private float maxGamePlayTimer = 10f; //this is going to be 5.00 minutes
    private Dictionary<ulong, bool> playerReadyDictionary;

    //network variables needed for synch the game states;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingOtherPlayers);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(4f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(300f);


    public bool HostDisconnected=false;
    // Game State Messages
    //[SerializeField] private GameObject ReadyPopUp;
    //[SerializeField] private GameObject WaitingOtherPlayer;
    //[SerializeField] private GameObject Countdown;
    //[SerializeField] private GameObject Timer;


    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    void Start()
    {
        // ReadyPopUp.SetActive(true);
        //WaitingOtherPlayer.SetActive(false);
        isConstructionWindowOpen = false;
        ReadyButtonPlayer();
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        if (IsHost)
        {
            Debug.Log("CLIENT DISCONNETED");
            ActivateDisconnectStateServerRpc();
        }
        else
        {
            HostDisconnected = true;
        }
    }

    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += State_OnValueChanged;
        if (IsServer)
        {
            //NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
           Vector3 position= spawnPositionList[ConnectionManager.Instance.GetPlayerDataIndexFromClientId(clientId)].position;
            Transform playerTransform = Instantiate(playerPrefab, position, Quaternion.identity);

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
             
        }
    }

    public void ReadyButtonPlayer()
    {
        if (state.Value == State.WaitingOtherPlayers)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("START WITH THIS");
          //  ReadyPopUp.SetActive(false);
           // WaitingOtherPlayer.SetActive(true);
            SetPlayerReadyServerRpc();
        }
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientsReady = true;
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playerReadyDictionary.ContainsKey(clientId) || !playerReadyDictionary[clientId])
            {
                // This player is NOT ready
                allClientsReady = false;
                break;
            }
        }

        if (allClientsReady)
        {
            Debug.Log("ALL READY");
            showGameClientRpc();
            state.Value = State.CountdownToStart;
        }
    }
    [ClientRpc]
    private void showGameClientRpc()
    {
       Debug.Log("SHOW COUTDONW TO PLAYER");
    }
    void Update()
    {
        if (!IsHost)
        {
            return;
        }
        switch (state.Value)
        {
            case State.WaitingOtherPlayers:
                break;
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                //Debug.Log(countdownToStartTimer.Value);
                if (countdownToStartTimer.Value < 1f)
                {
                    Debug.Log("START GAM£");
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = maxGamePlayTimer;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                //Debug.Log(gamePlayingTimer.Value);
                if (gamePlayingTimer.Value <= 0f || gameObject.GetComponent<GameManager>().GetEvidence()>=5)
                {
                    state.Value = State.GameEnd;
                }
                break;
            case State.GameEnd:
                {
                   // Debug.Log("GAME OVER");
                }
                break;
            case State.PlayerDisconnected:
                {
                    visualDebugger.AddMessage("DISCONNECTED PLAYER");
                    break;
                }
        }
    }
    public bool IsGamePlaying()
    {
        return state.Value == State.GamePlaying;
    }
    public bool IsCountdownToStartActive()
    {
        return state.Value == State.CountdownToStart;
    }
    public string GetCountdownToStartTimer()
    {
        if (countdownToStartTimer.Value > 0)
        {
            string seconds = Mathf.Floor(countdownToStartTimer.Value % 60).ToString("0");
            return seconds;
        }
        else
        {
            return "0";
        }
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameEnd;
    }
    public bool IsPlayerDisconnected()
    {
        return state.Value == State.PlayerDisconnected;
    }
    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingOtherPlayers;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    public string GetGamePlayingTimerNormalized()
    {
        if (gamePlayingTimer.Value > 0f)
        {
           

            string minutes = Mathf.Floor(gamePlayingTimer.Value / 60).ToString("00");
            string seconds = Mathf.Floor(gamePlayingTimer.Value % 60).ToString("00");

            return minutes + ":" + seconds;
        }
        else
        {
            return 00 + ":" + 00;
        }
    }

    public string GetCurrentScene()
    {
        return state.Value.ToString();
    }

    public void SetConstructionWindowActive(bool value)
    {
        
        isConstructionWindowOpen = value;
        //Debug.Log(isConstructionWindowOpen);
    }
    public bool GetConstructionWindowActive()
    {
        //Debug.Log(isConstructionWindowOpen);
        return isConstructionWindowOpen;
    }

    public bool CloseConstructionWindow()
    {
        return false;
    }

    public bool CanMovePlayer()
    {
        return IsGamePlaying() && !GetConstructionWindowActive();
    }



    // detect disconnection during game
    [ServerRpc(RequireOwnership = false)]
    private void ActivateDisconnectStateServerRpc()
    {
        Debug.Log("CALLED RPC");
        state.Value = State.PlayerDisconnected;
        ActivateDisconnectStateClientRpc();
    }

    [ClientRpc]
    private void ActivateDisconnectStateClientRpc()
    {
        visualDebugger.AddMessage("SHOW THIS WHEN CLIENT DISCONEETS");
    }
    

   
}
