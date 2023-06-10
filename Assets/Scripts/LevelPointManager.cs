using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPointManager : MonoBehaviour
{
    public string levelLabel;
    public string levelName;

    public bool unlocked;

    void Start()
    {
        //unlock level if should be unlocked
    }

    
    void Update()
    {
        
    }

    public void SelectLevel() {
        Debug.Log($"Selected level: {levelName}");
    }

    public string GetLabel() {
        return levelLabel;
    }

    public string GetName() {
        return levelName;
    }

    public bool IsUnlocked() {
        return unlocked;
    }
}
