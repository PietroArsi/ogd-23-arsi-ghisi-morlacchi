using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManagementUI : MonoBehaviour
{
    
    [SerializeField] private Button CreateLobby;
    [SerializeField] private Button ReturnMenu;

   
    [SerializeField] private TMP_InputField nameLobbyInputs ;

    public bool isActive = false;

    private void Awake()
    {
            isActive = true;
        CreateLobby.onClick.AddListener(() =>
        {

            CatnipLobby.Instance.CreateLobby(nameLobbyInputs.text);
        });
        ReturnMenu.onClick.AddListener(() =>
        {

            SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
        });
        //}

        //  JoinGameButton.onClick.AddListener(() =>
        // {
        //ConnectionManager.Instance.StartClient();
        //    CatnipLobby.Instance.QuickJoin();
        //});
    }

}
