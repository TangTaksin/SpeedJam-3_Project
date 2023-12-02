using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_UI : MonoBehaviour
{
    [SerializeField] HP_Icon[] healthIcons;
    [SerializeField] Hitpoint playerHp;

    private void Start()
    {
        if (playerHp != null)
            playerHp.onHealthChanged += UpdateUI;
    }

    void UpdateUI(int hp)
    {
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i + 1 > hp)
                healthIcons[i].SetState(false);
            else
                healthIcons[i].SetState(true);
        }
    }
}
