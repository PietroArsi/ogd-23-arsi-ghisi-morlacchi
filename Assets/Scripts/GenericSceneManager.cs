using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSceneManager : MonoBehaviour
{
    public void ChangeScene(string name) {
        SceneLoader.LoadSceneByName(name);
    }

    public void CreateLobby() {
        HostOrJoin.pressHostBtn();
        SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
    }
    public void JoinLobby()
    {
        HostOrJoin.pressJoinBtn();
        SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
    }  
}