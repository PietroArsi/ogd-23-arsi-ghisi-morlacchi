using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    public MusicManager.track music;

    void Start()
    {
        GameObject.Find("MusicManager").GetComponent<MusicManager>().StartMusic(music);
    }
}
