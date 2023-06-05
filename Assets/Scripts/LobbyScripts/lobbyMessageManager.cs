using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class lobbyMessageManager : MonoBehaviour
{
    [SerializeField] private Image messagesWarnigns;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button close;

    private void Awake()
    {
        close.onClick.AddListener(Hide);
    }
    // Start is called before the first frame update
    void Start()
    {
        ConnectionManager.Instance.OnFailToJoinGame += ConnectionManager_OnFailedToJoinGame;
        CatnipLobby.Instance.OnCreateLobbyStarted += CatnipLobby_OnCreateLobbyStarted;
        CatnipLobby.Instance.OnCreateLobbyFailed += CatnipLobby_OnCreateLobbyFailed;
        CatnipLobby.Instance.OnJoinStarted += CatnipLobby_OnJoinStarted;
        CatnipLobby.Instance.OnJoinFailed += CatnipLobby_OnJoinFailed;
        Hide();
    }

    private void CatnipLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join Lobby");
    }

    private void CatnipLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby");
    }

    private void CatnipLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create lobby");
    }

    private void CatnipLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating lobby ....");
    }

    private void ConnectionManager_OnFailedToJoinGame(object sender, EventArgs e)
    {
        if (NetworkManager.Singleton.DisconnectReason == "")
        {
            ShowMessage("FailedToConnect");
            
        }
        else
        {
            ShowMessage(NetworkManager.Singleton.DisconnectReason);
        }
    }

    private void ShowMessage(string message)
    {
        Show();
        messageText.text = message;
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame

    private void OnDestroy()
    {
        ConnectionManager.Instance.OnFailToJoinGame -= ConnectionManager_OnFailedToJoinGame;
        CatnipLobby.Instance.OnCreateLobbyStarted -= CatnipLobby_OnCreateLobbyStarted;
        CatnipLobby.Instance.OnCreateLobbyFailed -= CatnipLobby_OnCreateLobbyFailed;
        CatnipLobby.Instance.OnJoinStarted -= CatnipLobby_OnJoinStarted;
        CatnipLobby.Instance.OnJoinFailed -= CatnipLobby_OnJoinFailed;
    }
}
