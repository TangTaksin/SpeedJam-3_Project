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
        if (ComboGuage != null)
        {
            ComboGuage.gameObject.SetActive(false);
        }

        GameManager.onComboTimerUpdated += ComboMeterUpdate;
        GameManager.onScoreUpdated += ScoreUpdate;
    }

    void ScoreUpdate(float score, float combo)
    {
        if (ScoreTxt != null && ComboTxt != null)
        {
            ScoreTxt.text = string.Format("{0:#,0}", score);
            ComboTxt.text = string.Format("x{0,0}", combo);
        }
    }

    void ComboMeterUpdate(float comboTime, float maxTime, bool isComboing)
    {
        if (ComboGuage != null)
        {
            ComboGuage.gameObject.SetActive(isComboing);
            ComboGuage.value = comboTime / maxTime;
        }
    }
}
