using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

/// <summary>
/// If we decide the charcter selection is before the lobby scene 
/// lets make it single player, if not we do it basing form this function
/// for the tutorial
/// </summary>
public class CharacterSelectReady : NetworkBehaviour
{
    private Dictionary<ulong, bool> playerReadyDictionary;

    //gonna add thhis intance later when we need it 
    //public static CharacterSelectReady Instance (get;private set)
    private void Awake()
    {
        //Instance=this
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void thisPlayerReady()
    {
        SetPlayerReadyServerRpc();
    }
    
    //the client tell the host/server that is ready
    // if all the players are ready load the gameScene
    [ServerRpc(RequireOwnership =false)]
    private void SetPlayerReadyServerRpc(ServerRpcParams serverRpcParams = default)
    {
        playerReadyDictionary[serverRpcParams.Receive.SenderClientId] = true;

        bool allClientReady = true;
        foreach(ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if(!playerReadyDictionary.ContainsKey(clientId)|| playerReadyDictionary[clientId])
            {
                //find if the client is not ready
                allClientReady = false;
                break;
            }
        }
        if (allClientReady)
        {
            //load a the scene
            //SceneLoader.LoadNetwork()
        }
    }
}
