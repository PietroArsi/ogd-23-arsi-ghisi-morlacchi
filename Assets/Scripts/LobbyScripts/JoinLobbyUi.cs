using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinLobbyUi : MonoBehaviour
{
    public static JoinLobbyUi LocalInstance { get; private set; }
    // [SerializeField] private Button CreateLobby; //change scene make or make a pop up of text and stuff doe the name and other (gonna do this tommorow
    [SerializeField] private Button JoinGameButton; //change scene with all the lobbies active see other code for this.

    // this is gonna be transfer in a new scene depends on what pietro does
    //[SerializeField] private TMP_InputField nameLobby;
    //[SerializeField] private Button createLobby;
    public bool isActive = false;

    private void Awake()
    {
        //if (CreateLobbyUI.LocalInstance.isActive) {
          //  isActive = false;
         //   this.gameObject.SetActive(false);
       // }
     //   else
       // {
            isActive = true;
            JoinGameButton.onClick.AddListener(() =>
            {
            //ConnectionManager.Instance.StartHost();
            //SceneLoader.LoadNetwork(SceneLoader.Scene.CharacterSelectionTest);
            CatnipLobby.Instance.QuickJoin();
            });

            //  JoinGameButton.onClick.AddListener(() =>
            // {
            //ConnectionManager.Instance.StartClient();
            //    CatnipLobby.Instance.QuickJoin();
            //});
       // }
    }

}
