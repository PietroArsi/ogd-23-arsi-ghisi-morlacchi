using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSceneManager : MonoBehaviour
{
    public void ChangeScene(string name) {
        SceneLoader.LoadSceneByName(name);
    }
}
