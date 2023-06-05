using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class CharacterSelectUI : MonoBehaviour
{
    //see tutorial later for more complex when all the player are ready.
    [SerializeField] private Button startButton;
    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private Button Menu;
    // Start is called before the first frame update
    void Awake()
    {
        startButton.onClick.AddListener(()=>{


            CatnipLobby.Instance.DeleteLobby();
            //ConnectionManager.Instance.StartGame();
            SceneLoader.LoadNetwork(SceneLoader.Scene.SceneFlowLevel);
           
            ConnectionManager.Instance.spawnPlayers();
        });
        show();
        Menu.onClick.AddListener(() => {
       
            NetworkManager.Singleton.Shutdown();
           
            if (NetworkManager.Singleton.IsHost)
            {
                Debug.Log("THIS IS DELETED");
                CatnipLobby.Instance.DeleteLobby();
            }
            else
            {
                CatnipLobby.Instance.LeaveLobby();
            }
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
            ConnectionManager.Instance.spawnPlayers();
        });
    }

    private void Start()
    {
        Lobby lobby = CatnipLobby.Instance.GetLobby();
        Debug.Log(lobby.Name);
        lobbyNameText.text = "Lobby Name: " + lobby.Name;
    }
    private void show()
    {
        if (!NetworkManager.Singleton.IsHost)
        {
            startButton.gameObject.SetActive(false);
        }
    }

}
