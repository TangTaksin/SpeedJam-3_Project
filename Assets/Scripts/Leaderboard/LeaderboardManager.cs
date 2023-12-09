using System.Collections.Generic;
using UnityEngine;
using Dan.Main;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    // UI elements to display leaderboard data
    [SerializeField] private List<TextMeshProUGUI> names;       // List of TextMeshProUGUI for displaying usernames
    [SerializeField] private List<TextMeshProUGUI> scores;      // List of TextMeshProUGUI for displaying scores
    [SerializeField] private TMP_InputField _usernameInput;    // Input field for entering a username

    // Called when the script starts
    private void Start()
    {
        // Load and display the initial leaderboard entries
        LoadEntries();
    }

    // Method to load leaderboard entries
    private void LoadEntries()
    {
        // Q: How do I reference my own leaderboard?
        // A: Leaderboards.<NameOfTheLeaderboard>

        // Retrieve entries from the leaderboard asynchronously
        Leaderboards.GravitonGrapplerLeaderboard.GetEntries(entries =>
        {
            // Log the number of entries retrieved
            Debug.Log($"Retrieved {entries.Length} entries from the leaderboard.");

            // Determine the loop length based on the minimum of entries and UI elements count
            int loopLength = Mathf.Min(entries.Length, names.Count);

            // Update UI elements with retrieved data
            for (int i = 0; i < loopLength; i++)
            {
                names[i].text = entries[i].Username;
                scores[i].text = entries[i].Score.ToString();
            }

            // Log success message
            Debug.Log("Leaderboard entries loaded.");
        });
    }

    // Method to upload a new entry to the leaderboard
    public void UploadEntry(string username, int score)
{
    // Upload new entry asynchronously
    Leaderboards.GravitonGrapplerLeaderboard.UploadNewEntry(username, score, 
        entries => 
        {
            // Check if the upload was successful
            if (entries)
            {
                // Log success message
                Debug.Log("Leaderboard entry uploaded successfully.");

                // Reload entries to update the UI
                LoadEntries();
            }
            else
            {
                // Log error message if the upload fails
                Debug.LogError("Failed to upload leaderboard entry.");
            }
        },
        error =>
        {
            // Log error message if there is an issue with the upload
            Debug.LogError($"Error uploading leaderboard entry: {error}");
        });
}

}
