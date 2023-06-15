using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementCard : MonoBehaviour
{
    public TMPro.TextMeshProUGUI descriptionText;
    public Button button;
    public string achievementName;

    private string originalDecription;

    void Start() {
        originalDecription = descriptionText.text;    
    }

    public void SetUnlocked(bool mode) {
        if (mode) {
            descriptionText.text = originalDecription;
            button.interactable = true;
        } else {
            descriptionText.text = "???";
            button.interactable = false;
        }
    }

    public string GetAchievementName() {
        return achievementName;
    }
}
