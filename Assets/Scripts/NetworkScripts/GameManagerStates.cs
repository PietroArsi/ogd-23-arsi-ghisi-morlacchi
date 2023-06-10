using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManagerStates : NetworkBehaviour
{
    public static GameManagerStates Instance { get; private set; }
    [SerializeField] private Transform playerPrefab;
    //varoious GameState;
    private enum State
    {
        WaitingOtherPlayers,
        CountdownToStart,
        GamePlaying,
        GameEnd,
    }
    public event EventHandler OnStateChanged;
    public event EventHandler OnLocalPlayerReadyChanged;

    private bool isLocalPlayerReady;
    [SerializeField] private float maxGamePlayTimer = 10f; //this is going to be 5.00 minutes
    private Dictionary<ulong, bool> playerReadyDictionary;

    //network variables needed for synch the game states;
    private NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingOtherPlayers);
    private NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    private NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(0f);

    // Game State Messages
    //[SerializeField] private GameObject ReadyPopUp;
   // [SerializeField] private GameObject WaitingOtherPlayer;
    //[SerializeField] private GameObject Countdown;
    //[SerializeField] private GameObject Timer;


    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    void Start()
    {
        //ReadyPopUp.SetActive(true);
        //WaitingOtherPlayer.SetActive(false);

    }

    public override void OnNetworkSpawn()
    {
      //  state.OnValueChanged += State_OnValueChanged;
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
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
        state.Value = State.CountdownToStart;
    }
    
    public void ReadyButtonPlayer()
    {
        if (state.Value == State.WaitingOtherPlayers)
        {
            isLocalPlayerReady = true;
            OnLocalPlayerReadyChanged?.Invoke(this, EventArgs.Empty);
            //ReadyPopUp.SetActive(false);
            //WaitingOtherPlayer.SetActive(true);
            //SetPlayerReadyServerRpc();
        }
    }
    /*
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
            showGameClientRpc();
            Debug.Log("ALL READY");
            state.Value = State.CountdownToStart;
        }
    }
    [ClientRpc]
    private void showGameClientRpc()
    {
        WaitingOtherPlayer.SetActive(false);
    }
    */
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
                Debug.Log(countdownToStartTimer.Value);
                if (countdownToStartTimer.Value < 0f)
                {
                    Debug.Log("START GAM£");
                    state.Value = State.GamePlaying;
                    gamePlayingTimer.Value = maxGamePlayTimer;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                Debug.Log(gamePlayingTimer.Value);
                if (gamePlayingTimer.Value < 0f)
                {
                    state.Value = State.GameEnd;
                }
                break;
            case State.GameEnd:
                {
                    Debug.Log("GAME OVER");
                }
                break;
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
    public float GetCountdownToStartTimer()
    {
        return countdownToStartTimer.Value;
    }
    public bool IsGameOver()
    {
        return state.Value == State.GameEnd;
    }

    public bool IsWaitingToStart()
    {
        return state.Value == State.WaitingOtherPlayers;
    }

    public bool IsLocalPlayerReady()
    {
        return isLocalPlayerReady;
    }
    public float GetGamePlayingTimerNormalized()
    {
        return 1 - (gamePlayingTimer.Value / maxGamePlayTimer);
    }

    public string GetCurrentScene()
    {
        return state.Value.ToString();
    }
   
}
