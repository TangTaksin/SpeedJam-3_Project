using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionSetting : MonoBehaviour
{
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private GameObject startPanel;
    [SerializeField] private GameObject leaderboardPanel;
    [SerializeField] private GameObject inputPlayer;
    [SerializeField] private string nextSceneName;

    private void Start()
    {
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }


    private void TogglePanel()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            optionPanel.SetActive(!optionPanel.activeSelf);
            Time.timeScale = optionPanel.activeSelf ? 0f : 1f; //// Pause and Resume the game
        }
    }

    public void PlayGame()
    {
        //Load New Game
        SceneManager.LoadScene(nextSceneName);
    }

    public void PlayerAgain()
    {
        Time.timeScale = 1f;
        leaderboardPanel.SetActive(false);
        inputPlayer.SetActive(false);
        //Load New Game
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void CloseOptionPanel()
    {
        optionPanel.SetActive(!optionPanel.activeSelf);
        Time.timeScale =  1f;

    }

    public void BackToMenu()
    {
        optionPanel.SetActive(false);
        creditsPanel.SetActive(false);
        startPanel.SetActive(true);

    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
        startPanel.SetActive(false);

    }

    public void ShowOptionPanel()
    {
        optionPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
}
