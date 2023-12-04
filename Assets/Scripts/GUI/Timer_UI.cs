using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Timer;

    private void Start()
    {
        GameManager.onGameTimerUpdated += UpdateTimer;
    }

    void UpdateTimer(float time)
    {
        var timeSpan = TimeSpan.FromSeconds(time);
        Timer.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
    }
}
