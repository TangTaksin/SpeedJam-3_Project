using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    [SerializeField] private TMP_InputField _usernameInput;

    private void Start()
    {
        LoadEntries();
    }

    private void LoadEntries()
    {
        // Q: How do I reference my own leaderboard?
        // A: Leaderboards.<NameOfTheLeaderboard>

        Leaderboards.GravitonGrapplerLeaderboard.GetEntries(entries =>
        {
            Debug.Log($"Retrieved {entries.Length} entries from the leaderboard.");
            
            int loopLength = Mathf.Min(entries.Length, names.Count);
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = entries[i].Username;
                scores[i].text = entries[i].Score.ToString();
            }

            Debug.Log("Leaderboard entries loaded.");
        });
    }

    public void UploadEntry(string username, int score)
    {
        Leaderboards.GravitonGrapplerLeaderboard.UploadNewEntry(username, score, entries =>
        {
            if (entries)
            {
                Debug.Log("Leaderboard entry uploaded successfully.");
                LoadEntries();
            }
            else
            {
                Debug.LogError("Failed to upload leaderboard entry.");
            }
        });
    }
}
