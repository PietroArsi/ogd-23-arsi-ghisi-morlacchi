using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostDisconnetUI : MonoBehaviour
{
    [SerializeField] private Button returnMenu;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private GameObject DisconnectionUI;

   

    private void Awake()
    {
        returnMenu.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void Update()
    {
        //Debug.Log("HAS PLAYER DISCONECTED? " + GameManagerStates.Instance.IsPlayerDisconnected());
        if (!GameManagerStates.Instance.IsGameOver())
        {
            if (!GameManagerStates.Instance.IsPlayerDisconnected())
            {
                return;
            }
            else
            {
                Show("Client has disconneted");
            }
        }
        else {
            visualDebugger.AddMessage("Don't show this message");
        }
    }
    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("DISCONECT");

        if (clientId == NetworkManager.ServerClientId && !GameManagerStates.Instance.IsGameOver())
        {

            //Server is shutting down
            Show("Host has disconnected");
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


    [ServerRpc (RequireOwnership =false)]
    private void HandleClientDisconnetServerRpc(ulong clientId)
    {

        HandleClientDisconnetClientRpc(clientId);

    }

    [ClientRpc]
    private void HandleClientDisconnetClientRpc(ulong clientId)
    {
       
       
    }
}

   
