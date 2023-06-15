using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSceneManager : MonoBehaviour
{
    private Animator transition;

    void Start() {
        transition = GameObject.Find("LevelLoader").transform.Find("CircleWipe").GetComponent<Animator>();
    }

    public void ChangeScene(string name) {
        StartCoroutine(LoadScene(name));
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
    
    IEnumerator LoadScene(string name) {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1f);
        SceneLoader.LoadSceneByName(name);
    }
}