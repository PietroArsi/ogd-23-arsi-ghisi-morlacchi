using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnetUI : MonoBehaviour
{
    [SerializeField] private Button returnMenu;

    private bool IsHostDisconneted;

    private void Awake()
    {
        returnMenu.onClick.AddListener(() =>
        {

            if (IsHostDisconneted)
            {
                SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
            }
            else
            {
                NetworkManager.Singleton.Shutdown();
                SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
            }
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
       // if (clientId == NetworkManager.ServerClientId)
        //{

            //Server is shutting down
            Show();
        //}
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
        NetworkManager.Singleton.OnClientDisconnectCallback -= NetworkManager_OnClientDisconnectCallback;
    }
}

   
