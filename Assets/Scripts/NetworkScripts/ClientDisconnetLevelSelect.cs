using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientDisconnetLevelSelect : NetworkBehaviour
{
    public static ClientDisconnetLevelSelect Instance { get; private set; }
    public bool HostDisconnected;
    private enum State
    {
        Default,
        PlayerDisconnected,
    }
    public event EventHandler OnStateChanged;

    private NetworkVariable<State> state = new NetworkVariable<State>(State.Default);

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        HostDisconnected = false;
        state.OnValueChanged += State_OnValueChanged;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state.Value)
        {
            case State.Default:
                break;
            case State.PlayerDisconnected:
                break;
                
        }
    }
    private void State_OnValueChanged(State previousValue, State newValue)
    {
        OnStateChanged?.Invoke(this, EventArgs.Empty);
    }
    public bool IsPlayerDisconnected()
    {
        return state.Value == State.PlayerDisconnected;
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        if (IsHost)
        {
            ActivateDisconnectStateServerRpc();
        }
        else
        {
            HostDisconnected = true;
        }
    }

    // detect disconnection during game
    [ServerRpc(RequireOwnership = false)]
    private void ActivateDisconnectStateServerRpc()
    {
        state.Value = State.PlayerDisconnected;
        ActivateDisconnectStateClientRpc();
    }

    [ClientRpc]
    private void ActivateDisconnectStateClientRpc()
    {
        Debug.Log("HELLO");
        //visualDebugger.AddMessage("SHOW THIS WHEN CLIENT DISCONEETS");
    }

    public override void OnDestroy()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }
}
