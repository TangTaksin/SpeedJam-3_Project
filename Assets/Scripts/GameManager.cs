using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    float score = 0;

    [SerializeField] float gameTimeLimit;
    float gameTime;

    [SerializeField] int comboLimit = 999;
    int comboCount;
    int highestCombo;

    [SerializeField] float comboBeginTimer;
    float comboTimer;

    bool isComboing;

    bool isPause;

    public delegate void OnScoreUpdated(float currentScore, float comboCount);
    public static event OnScoreUpdated onScoreUpdated;
    public static event OnScoreUpdated onGameEnded;

    public delegate void OnComboTimerUpdate(float currentTimer, float maxTimer, bool isComboing);
    public static event OnComboTimerUpdate onComboTimerUpdated;

    public delegate void OnTimerUpdate(float currentTimer);
    public static event OnTimerUpdate onGameTimerUpdated;

    void onHit(float score)
    {
        //reset combo timer if ongoing
        if (isComboing)
            comboTimer = comboBeginTimer;

        //add score
        addScore(score);
    }

    void onKill(float score)
    {
        //increase combo count by 1 if it's less than combo limit
        if (comboCount < comboLimit)
            comboCount++;

        //start combo timer
        isComboing = true;
        comboTimer = comboBeginTimer;

        //add score
        addScore(score);
    }

    void addScore(float amount)
    {
        score += amount * comboCount;
        onScoreUpdated?.Invoke(score, comboCount);
    }


    private void Start()
    {
        gameTime = gameTimeLimit;
        onScoreUpdated?.Invoke(score, comboCount);
    }

    private void Update()
    {

        if (isPause)
            return;

        if (isComboing)
        {
            comboTimer -= Time.deltaTime;

            if (comboTimer <= 0)
            {
                isComboing = false;
                highestCombo = comboCount;
                comboCount = 0;
            }

            onComboTimerUpdated?.Invoke(comboTimer, comboBeginTimer, isComboing);
        }

        gameTime -= Time.deltaTime;
        onGameTimerUpdated(gameTime);
        if (gameTime <= 0)
        {
            onGameEnded.Invoke(score, highestCombo);
            gameTime = 0;
        }
    }

}
