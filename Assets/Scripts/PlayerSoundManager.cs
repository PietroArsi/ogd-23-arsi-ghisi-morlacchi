using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerSoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    public void PlaySound(string soundName, bool loop = false, float volume = 1f){
        //apply sound
        audioSource.loop = loop;
        audioSource.volume = volume;
        audioSource.Play();
    }
}
