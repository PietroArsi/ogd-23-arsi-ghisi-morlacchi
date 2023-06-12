using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using  UnityEngine.SceneManagement;

public static class SceneLoader
{
    //here we put the scene names when we have a more developed game
    public enum Scene
    {
        //temporary names
        Title,
        MainMenu,
        LobbyManagement,
        JoinLobby,
        CharacterSlectionScreen,
        LevelSelection,
        NetworkTestLevel,
    }

    //this when passing form main menu (is for single player loading screen)
    public static void LoadScene(Scene targetScene)
    {
        SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    }

    public static void LoadSceneByName(string targetScene) {
        SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
    }

    //this is to synch loading scene between host/server and clients
    public static void LoadNetwork(Scene targetScene)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(targetScene.ToString(), LoadSceneMode.Single);
    } 
}
