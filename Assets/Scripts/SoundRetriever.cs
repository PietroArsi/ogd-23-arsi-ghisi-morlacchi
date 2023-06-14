using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundRetriever : MonoBehaviour {
    private UISoundManager soundManager;

    void Start() {
        
    }

    void Update() {
        if (soundManager == null) {
            CheckReference();
        }
    }

    public void ClickSound(bool negative) {
        soundManager.ClickDefault(negative);
    }

    public void ClickSoundSpecific(AudioClip audio) {
        soundManager.ClickSound(audio);
    }

    private void CheckReference() {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("UI Sounds");

        foreach (GameObject obj in objs) {
            if (obj.GetComponent<UISoundManager>()) {
                soundManager = obj.GetComponent<UISoundManager>();
            }
        }
    }
}
