using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    private void Update()
    {
        timer.text = "Timer: " + GameManagerStates.Instance.GetGamePlayingTimerNormalized();
    }
}
