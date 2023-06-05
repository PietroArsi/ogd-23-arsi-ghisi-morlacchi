using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private Button GoToCreate;
    [SerializeField] private Button GoToJoin;
    //change scene make or make a pop up of text and stuff doe the name and other (gonna do this tommorow
    //[SerializeField] private Button GoToJoinScene; //change scene with all the lobbies active see other code for this.

    // this is gonna be transfer in a new scene depends on what pietro does
    //[SerializeField] private TMP_InputField nameLobby;
    //[SerializeField] private Button createLobby;


    private void Awake()
    {
        GoToCreate.onClick.AddListener(() =>
        {
            //ConnectionManager.Instance.StartHost();
            HostOrJoin.pressHostBtn();
            SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
            
           // CatnipLobby.Instance.CreateLobby("TestLobby");
        });
        GoToJoin.onClick.AddListener(() =>
        {
            //ConnectionManager.Instance.StartHost();
            HostOrJoin.pressJoinBtn();
            SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
            //ConnectionManager.Instance.GetJoinVisual();
            // CatnipLobby.Instance.CreateLobby("TestLobby");
        });
    }
    private void Start()
    {
        StartCoroutine(ClickEvents());
    }
    //temporary solution with keyboard
    IEnumerator ClickEvents()
    {
        while (true)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
                StopCoroutine(ClickEvents());
            }
            yield return null;
        }
    }
}
