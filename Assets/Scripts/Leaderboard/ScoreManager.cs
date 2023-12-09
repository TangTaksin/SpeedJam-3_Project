using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    // UI elements for displaying and inputting score data
    [SerializeField] private TextMeshProUGUI bestScore;           // TextMeshProUGUI for displaying the best score
    [SerializeField] private TMP_InputField inputPlayerName;      // Input field for entering player name

    // Event to notify when a score is submitted
    public UnityEvent<string, int> submitScoreEvent;

    // Subscribe to the onGameEnded event when the script starts
    private void Start()
    {
        GameManager.onGameEnded += GetScore;
    }

    // Method to get the final score when the game ends
    public void GetScore(float score, float highcombo)
    {
        // Update the bestScore UI text with the final score
        bestScore.text = score.ToString();
    }

    // Method to be called when the "Submit Score" button is clicked
    public void SubmitScore()
    {
        // Try to parse the best score from the UI text
        if (int.TryParse(bestScore.text, out int score))
        {
            // Log parsed score
            Debug.Log($"Parsed score: {score}");

            // Invoke the submitScoreEvent with player name and parsed score
            submitScoreEvent.Invoke(inputPlayerName.text, score);
        }
    }

}
