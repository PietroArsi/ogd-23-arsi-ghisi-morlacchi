using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericSceneManager : MonoBehaviour
{
    
    public void ChangeScene(string name) {
        SceneLoader.LoadSceneByName(name);
    }
    public void LoadNetwork(string targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }
    public void CreateLobby() {
        HostOrJoin.pressHostBtn();
        //ChangeScene(name);
        SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
    }
    public void JoinLobby( )
    {
        HostOrJoin.pressJoinBtn();
        SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
        //ChangeScene(name);
    }
    //public  void LoadScene(string targetScene)
    //{
    //    SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    //}


    //this is to synch loading scene between host/server and clients
    public void CreateLobbyGruop()
    {
        CatnipLobby.Instance.CreateLobby(NameGenerator.GenerateRandomName());
    }
    public void DeleteLobby()
    {
        CatnipLobby.Instance.DeleteLobby();
    }
    public void JoinSpecificLobby(Lobby lobby)
    {
        CatnipLobby.Instance.JoinwithId(lobby.Id);
    }
}