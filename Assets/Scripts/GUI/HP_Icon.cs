using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP_Icon : MonoBehaviour
{
    [SerializeField] Sprite _healthActive;
    [SerializeField] Sprite _healthInactive;

    Image _thisIcon;
    bool _isOn;

    private void Awake()
    {
        _thisIcon = GetComponent<Image>();

        if (_thisIcon == null)
        {
            Debug.LogError("Image component not found on HP_Icon.");
            // Handle the error or provide a default behavior.
        }

        if (_healthActive == null || _healthInactive == null)
        {
            Debug.LogError("Health sprites are not assigned on HP_Icon.");
        }
    }

    public void SetState(bool state)
    {
        _isOn = state;

        if (_thisIcon != null)
        {
            switch (_isOn)
            {
                case true:
                    if (_healthActive != null)
                        _thisIcon.sprite = _healthActive;
                    else
                        Debug.LogError("HealthActive sprite is not assigned on HP_Icon.");
                    break;
                case false:
                    if (_healthInactive != null)
                        _thisIcon.sprite = _healthInactive;
                    else
                        Debug.LogError("HealthInactive sprite is not assigned on HP_Icon.");
                    break;
            }
        }
        else
        {
            Debug.LogError("Image component is null on HP_Icon.");
        }
    }
}
