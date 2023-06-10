using System.Collections;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUi : MonoBehaviour
{
    public static JoinLobbyUi LocalInstance { get; private set; }
    // [SerializeField] private Button CreateLobby; //change scene make or make a pop up of text and stuff doe the name and other (gonna do this tommorow
    //[SerializeField] private Button JoinGameButton; //change scene with all the lobbies active see other code for this.
    [SerializeField] private Transform lobbyContainer;
    [SerializeField] private Transform lobbyTemplate;
    // this is gonna be transfer in a new scene depends on what pietro does


    private void Awake()
    {
        //if (CreateLobbyUI.LocalInstance.isActive) {
        //  isActive = false;
        //   this.gameObject.SetActive(false);
        // }
        //   else
        // {

        //JoinGameButton.onClick.AddListener(() =>
        //{
        //ConnectionManager.Instance.StartHost();
        //SceneLoader.LoadNetwork(SceneLoader.Scene.CharacterSelectionTest);
        // CatnipLobby.Instance.QuickJoin();
        //});

        //  JoinGameButton.onClick.AddListener(() =>
        // {
        //ConnectionManager.Instance.StartClient();
        //    CatnipLobby.Instance.QuickJoin();
        //});
        // }
        lobbyTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        CatnipLobby.Instance.OnLobbyListChanged += CatnipLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }
    private void CatnipLobby_OnLobbyListChanged(object sender, CatnipLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        //clean the lobby list
        //clean the lobby list

        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            //Debug.Log(lobby.Name);
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<lobbyTemplateUi>().setLobby(lobby);
        }

    }

    private void OnDestroy()
    {
        CatnipLobby.Instance.OnLobbyListChanged -= CatnipLobby_OnLobbyListChanged;
    }

}
