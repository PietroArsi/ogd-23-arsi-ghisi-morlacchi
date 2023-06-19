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

        if (GameManagerStates.Instance.IsCountdownToStartActive())
        {
            countDown.text = $"{GameManagerStates.Instance.GetCountdownToStartTimer()}";
        }
        //{
        //    countDown.gameObject.SetActive(false);
        //}

        else if(GameManagerStates.Instance.IsGamePlaying())
        {
            timer.text = $"{GameManagerStates.Instance.GetGamePlayingTimerNormalized()}";
        }
        
    }
}
