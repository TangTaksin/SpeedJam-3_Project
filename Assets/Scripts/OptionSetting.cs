using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionSetting : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;

    private void Start()
    {
        optionPanel.SetActive(false);
    }

    private void Update()
    {
        TogglePanel();

    }

    private void TogglePanel()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
            Time.timeScale = optionPanel.activeSelf ? 0f : 1f; //// Pause and Resume the game
        }

    }

    public void BackToMenu()
    {

    }
}
