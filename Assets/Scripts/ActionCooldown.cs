using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCooldown {
    private float remainingTime; 

    public ActionCooldown() {
        remainingTime = 0;
    }

    public void Set(float time) {
        remainingTime = time;
    }

    public void Advance(float elapsed) {
        remainingTime = Mathf.Max(remainingTime - elapsed, 0f);
    }

    public bool Check() {
        return remainingTime == 0f ? true : false;
    }
}
