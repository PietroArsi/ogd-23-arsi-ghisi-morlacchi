using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericSceneManager : MonoBehaviour
{
    private Animator transition;

    void Start() {
        transition = GameObject.Find("LevelLoader").transform.Find("CircleWipe").GetComponent<Animator>();
    }

    public void ChangeScene(string name) {
        StartCoroutine(LoadScene(name));
    }

    IEnumerator LoadScene(string name) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneLoader.LoadSceneByName(name);
    }

    public void LoadNetwork(string targetScene)
    {
        //NetworkManager.Singleton.SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        StartCoroutine(LoadSceneNetwork(targetScene));
    }

    IEnumerator LoadSceneNetwork(string name) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        NetworkManager.Singleton.SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void CreateLobby() {
        HostOrJoin.pressHostBtn();
        //ChangeScene(name);
        //SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
    }

    public void JoinLobby()
    {
        HostOrJoin.pressJoinBtn();
        //SceneLoader.LoadScene(SceneLoader.Scene.LobbyManagement);
    }
    
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