using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    private bool isPlaying;
    private track lastTrack;

    public enum track {
        PuzzlePieces,
        OutOfTime,
        Penultimate
    }

    void Awake() {
        isPlaying = false;

        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1) {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        
    }

    public void StartMusic(track music) {
        if (audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }

        if (!isPlaying || music != lastTrack) {
            if (music == track.PuzzlePieces) {
                audioSource.clip = Resources.Load<AudioClip>("Puzzle Pieces");
            } else if (music == track.OutOfTime) {
                audioSource.clip = Resources.Load<AudioClip>("Out of Time");
            } else if (music == track.Penultimate) {
                audioSource.clip = Resources.Load<AudioClip>("Penultimate");
            }

            isPlaying = true;
            lastTrack = music;
            audioSource.loop = true;
            audioSource.volume = 0.65f;
            audioSource.Play();
        }
    }

    public void StopMusic() {
        audioSource.Stop();
        isPlaying = false;
    }
}
