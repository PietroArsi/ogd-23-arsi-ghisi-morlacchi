using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI countDown;

    private void Update()
    {
        countDown.text="CountDown: " + GameManagerStates.Instance.GetCountdownToStartTimer();
        if (int.Parse(GameManagerStates.Instance.GetCountdownToStartTimer()) <= 0)
        {
            countDown.gameObject.SetActive(false);
        }
       
        timer.text = "Timer: " + GameManagerStates.Instance.GetGamePlayingTimerNormalized();
        
    }
}
