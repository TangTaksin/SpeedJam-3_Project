using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Combo_UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ComboTxt;
    [SerializeField] TextMeshProUGUI ScoreTxt;
    [SerializeField] Slider ComboGuage;

    private void Start()
    {
        GameManager.onComboTimerUpdated += ComboMeterUpdate;
        GameManager.onScoreUpdated += ScoreUpdate;
    }

    void ScoreUpdate(float score, float combo)
    {
        ScoreTxt.text = score.ToString();
        ComboTxt.text = string.Format("{0:0}x", combo);
    }

    void ComboMeterUpdate(float comboTime, float maxTime, bool isComboing)
    {
        ComboTxt.gameObject.SetActive(isComboing);

        ComboGuage.value = comboTime/ maxTime;
    }
}
