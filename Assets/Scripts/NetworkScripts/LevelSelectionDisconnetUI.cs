using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionDisconnetUI : MonoBehaviour
{
    [SerializeField] private Button returnMenu;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject DisconnectionUI;

    //for the level selection Scene
    [SerializeField] private bool detectedDisconnection;



    private void Awake()
    {
        //returnMenu.onClick.AddListener(() =>
        //{
        //    NetworkManager.Singleton.Shutdown();
        //    SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        //});
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        detectedDisconnection = false;
        Hide();
    }

    private void Update()
    {
     if (ClientDisconnetLevelSelect.Instance != null)
        {
            if (!ClientDisconnetLevelSelect.Instance.IsPlayerDisconnected())
            {
                return;
            }
            else
            {
                Show("Client has disconneted");

            }
        }
    }

    public void ReturnToMenu()
    {
        NetworkManager.Singleton.Shutdown();
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("DISCONECT");
        if (GameManagerStates.Instance != null)
        {
            if (clientId == NetworkManager.ServerClientId && !GameManagerStates.Instance.IsGameOver())
            {

                //Server is shutting down
                Show("Host has disconnected");
            }
        }
        else
        {
            if (clientId == NetworkManager.ServerClientId)
            {
                detectedDisconnection = true;
                //Server is shutting down
                Show("Host has disconnected");
            }
            //else
            //{
            //    if (NetworkManager.Singleton.IsHost)
            //    {
            //        HandleClientDisconnetClientRpc(clientId);
            //    }
            //}
        }

    }

    private void Show(string message)
    {
        messageText.text = message;
        DisconnectionUI.SetActive(true);
        // StartCoroutine(ReturnMenu());
    }

    private void Hide()
    {
        DisconnectionUI.SetActive(false);
    }

    //IEnumerator ReturnMenu()
    //{
    //    while (true)
    //    {
    //        if (Input.GetKey(KeyCode.Escape))
    //        {
    //            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
    //            StopCoroutine(ReturnMenu());
    //        }
    //        yield return null;
    //    }
    //}


    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            Debug.Log("<color=yellow> HostDisconnetUI called Destroy function OnClientDisconnetCallback Function</color>");
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }
    }


    [ServerRpc(RequireOwnership = false)]
    private void HandleClientDisconnetServerRpc(ulong clientId)
    {

        HandleClientDisconnetClientRpc(clientId);

    }

    [ClientRpc]
    private void HandleClientDisconnetClientRpc(ulong clientId)
    {
        detectedDisconnection = true;
        Show("Client has disconneted");

    }

    public bool DetectDisconnection()
    {
        return detectedDisconnection;
    }

}
