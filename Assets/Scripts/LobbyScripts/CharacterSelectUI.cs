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
    [SerializeField] private int connettedPlayers;
    // Start is called before the first frame update
    void Awake()
    {
        if (!NetworkManager.Singleton.IsHost)
            startButton.gameObject.SetActive(false);
        else
            startButton.enabled = true;
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_Client_OnClientConnetedCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Lobby lobby = CatnipLobby.Instance.GetLobby();
        Debug.Log("<color=yellow>CharacterSelectUI: Lobby Name: " + lobby.Name +  "</color>");
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
    }

    public void StartGame()
    {
        CatnipLobby.Instance.DeleteLobby();
        SceneLoader.LoadNetwork(SceneLoader.Scene.LevelSelection);
    }
    public void ReturnMenu()
    {
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
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong obj)
    {
        Debug.Log("<color=yellow>CharacterSelectUI OnClientDisconnetedCallback</color>");
        connettedPlayers--;
        startButton.enabled = true;
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
        
        if (NetworkManager.Singleton != null)
        {
            Debug.Log("<color=yellow>CharacterSelectUI remove event</color>");
            NetworkManager.Singleton.OnClientConnectedCallback -= NetworkManager_Client_OnClientConnetedCallback;
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
        else
        {
            Debug.Log("<color=yellow>CharacterSelectUI no event to remove</color>");
        }
    }


}
