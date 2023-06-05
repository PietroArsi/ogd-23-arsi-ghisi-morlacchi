using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cleanUpNetwork : MonoBehaviour
{

    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Debug.Log("NETMAN");
            Destroy(NetworkManager.Singleton.gameObject);
        }
        else
        {
            Debug.Log("NO CLEAN NM");
        }
        if (ConnectionManager.Instance != null)
        {
            Debug.Log("CONNETMAN");
            Destroy(ConnectionManager.Instance.gameObject);
        }
        else
        {
            Debug.Log("NO CLEAN CM");
        }
        if (CatnipLobby.Instance != null)
        {
            Debug.Log("CleanLobby");
            Destroy(CatnipLobby.Instance.gameObject);
        }
        else
        {
            Debug.Log("NO CLEAN Lobby");
        }
    }
    void Start()
    {
        //Set Cursor to not be visible
        Cursor.visible = true;
    }
}
