using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Icon : MonoBehaviour
{
    [SerializeField] Sprite healthActive;
    [SerializeField] Sprite healthInactive;

    Image thisIcon;
    bool isOn;

    private void Start()
    {
        thisIcon = GetComponent<Image>();
    }

    public void SetState(bool state)
    {
        isOn = state;

        switch (isOn)
        {
            case true:
                thisIcon.sprite = healthActive;
                break;
            case false:
                thisIcon.sprite = healthInactive;
                break;
        }
    }
}
