using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UISoundManager : MonoBehaviour
{
    private List<AudioClip> pianoNotes;
    public AudioClip normalClickSound;
    public AudioClip normalClickSoundNegative;
    private AudioSource audioSource;

    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI Sounds");

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ClickNote(bool usePiano) {
        CheckReference();

        audioSource.loop = false;
        if (usePiano) {
            audioSource.clip = pianoNotes[Random.Range(0, pianoNotes.Count)];
        } else {
            audioSource.clip = normalClickSound;
        }

        audioSource.volume = 0.5f;
        audioSource.Stop();
        audioSource.Play();
    }

    public void ClickDefault(bool negative) {
        CheckReference();

        audioSource.loop = false;
        if (negative) {
            audioSource.clip = normalClickSoundNegative;
        }
        else {
            audioSource.clip = normalClickSound;
        }

        audioSource.volume = 0.45f;
        audioSource.Stop();
        audioSource.Play();
    }

    public void ClickSound(AudioClip audio) {
        CheckReference();

        audioSource.loop = false;
        audioSource.clip = audio;
        audioSource.volume = 0.45f;
        audioSource.Stop();
        audioSource.Play();
    }

    private void CheckReference() {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }
}
