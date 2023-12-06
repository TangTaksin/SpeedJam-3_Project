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

    void UpdateUI(int hp, int max)
    {
        Debug.Log("Updating UI with HP: " + hp + " / " + max);

        if (healthIcons != null)
        {
            for (int i = 0; i < healthIcons.Length; i++)
            {
                // Ensure healthIcons[i] is not null
                if (healthIcons[i] != null)
                {
                    Debug.Log("Updating UI for healthIcon[" + i + "]");

                    if (i + 1 > hp)
                        healthIcons[i].SetState(false);
                    else
                        healthIcons[i].SetState(true);
                }
                else
                {
                    // Handle null healthIcons[i]
                    Debug.LogError("healthIcons[" + i + "] is null");
                }
            }
        }
        else
        {
            // Handle null healthIcons
            Debug.LogError("healthIcons is null");
        }
    }

    private void OnDestroy()
    {
        if (playerHp != null)
            playerHp.onHealthChanged -= UpdateUI;
    }
}
