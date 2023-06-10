using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostLobbyDisconnection : MonoBehaviour
{
    [SerializeField] private Button returnMenu;



    private void Awake()
    {
        returnMenu.onClick.AddListener(() =>
        {


            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("HELLO");
        
        if (clientId == NetworkManager.ServerClientId)
        {

            //Server is shutting down
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
        StartCoroutine(ReturnMenu());
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ReturnMenu()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
                StopCoroutine(ReturnMenu());
            }
            yield return null;
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            Debug.Log("<color=yellow> HostLobbyDisconnet called Destroy function OnClientDisconnetCallback Function</color>");
            NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
        }

    }
}
