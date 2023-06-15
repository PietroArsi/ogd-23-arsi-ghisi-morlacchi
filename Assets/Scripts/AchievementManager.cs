using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public List<Transform> achievements;

    void Start()
    {
        foreach(Transform ach in achievements) {
            bool check = SaveManager.CheckAchievement(ach.GetComponent<AchievementCard>().GetAchievementName());
            ach.GetComponent<AchievementCard>().SetUnlocked(check);
        }
    }
}
