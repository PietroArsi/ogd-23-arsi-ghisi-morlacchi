using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;
using System;

public class CharacterSelectUI : MonoBehaviour
{
    //see tutorial later for more complex when all the player are ready.
    [Header("Character Menu UI")]
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private Button Menu;
    [SerializeField] private int connettedPlayers;
    // Start is called before the first frame update
    void Awake()
    {
        startButton.onClick.AddListener(()=>{


            CatnipLobby.Instance.DeleteLobby();
            //ConnectionManager.Instance.StartGame();
            SceneLoader.LoadNetwork(SceneLoader.Scene.NetworkTestLevel);
           
            ConnectionManager.Instance.spawnPlayers();
        });

        if (!NetworkManager.Singleton.IsHost)
            startButton.gameObject.SetActive(false);
        else
            startButton.enabled = false;
        Menu.onClick.AddListener(() => {
       
            NetworkManager.Singleton.Shutdown();
           
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("<color:yellow>CharacterSlectUI DELETE LOBBY </color>");
                CatnipLobby.Instance.DeleteLobby();
            }
            else
            {
                Debug.Log("<color:yellow>CharacterSlectUI LEAVE LOBBY</color>");
                CatnipLobby.Instance.LeaveLobby();
            }
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
            ConnectionManager.Instance.spawnPlayers();
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnetedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Lobby lobby = CatnipLobby.Instance.GetLobby();
        Debug.Log("<color=yellow>CharacterSelectUI: Lobby Name: " + lobby.Name +  "</color>");
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        Debug.Log("<color=yellow>CharacterSelectUI OnClientDisconnetedCallback</color>");
        connettedPlayers--;
        startButton.enabled = false;
        startButton.gameObject.SetActive(false);
    }

    private void NetworkManager_Client_OnClientConnetedCallback(ulong obj)
    {
        
        Debug.Log("<color=yellow>CharacterSelectUI OnClientConnetedCallback</color>");
        connettedPlayers++;
        if (connettedPlayers == ConnectionManager.MAX_NUMBER_PLAYER-1 && NetworkManager.Singleton.IsHost)
        {
            startButton.enabled = true;
        }
    }

    private void OnDestroy()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_Client_OnClientConnetedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }


}
