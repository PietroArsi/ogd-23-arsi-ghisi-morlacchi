using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Unity.Netcode;
public class NetworkManagerUi : NetworkBehaviour
{
    [SerializeField] private Button hostButn;
    [SerializeField] private Button clientBtn;

    private void Awake()
    {
        //delegates
        
        hostButn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
        });
        clientBtn.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
        });
    }
}
