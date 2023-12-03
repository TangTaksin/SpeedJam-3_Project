using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bestScore;
    [SerializeField] private TMP_InputField inputPlayerName;

    public UnityEvent<string, int> submitScoreEvent;

    public void SubmitScore()
    {
        if (int.TryParse(bestScore.text, out int score))
        {
            Debug.Log($"Parsed score: {score}");
            submitScoreEvent.Invoke(inputPlayerName.text, score);
        }
        else
        {
            Debug.LogError("Invalid score format. Unable to parse the score.");
        }
    }
}
