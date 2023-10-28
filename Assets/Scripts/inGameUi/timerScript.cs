using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class timerScript : MonoBehaviour
{
    Image timerBar;
    public float maxTime = 10f;
    float timeLeft;

    bool timerOn;
    

    private void OnEnable()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
        timerOn = true;
    }

    private void Update()
    {
        updateTimer();
    }

    void updateTimer()
    {
        if (timerOn)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                timerBar.fillAmount = timeLeft / maxTime;

            }
            else
            {
                timeLeft = 0;
                timerOn = false;
                shiftTurnByTimer();
            }
        }
       
    }

    void shiftTurnByTimer()
    {
        RummyManager.instance.shiftTurnByTimer(RummyManager.instance.GetPlayerIndexInList(RummyManager.instance.GetActivePlayer));
    }
}
