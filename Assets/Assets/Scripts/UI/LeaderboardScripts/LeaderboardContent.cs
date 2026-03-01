using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.UI;
public class LeaderboardEntry
{
    public string username;
    public string hero;
    public string difficulty;
    public int score;
    public int timeSurvivedSeconds;
    public long timestamp;
}
public class LeaderboardContent : MonoBehaviour
{
    [SerializeField] private GameObject placementGO;
    [SerializeField] private Color[] colors;
    [SerializeField] private Sprite[] medals;
    [SerializeField] private Sprite topPlaceSprite;
    [SerializeField] private Sprite otherPlaceSprite;
    [SerializeField] private GameObject content;

    private const string MAP_NAME = "SkeletonMap";

    private void OnEnable()
    {
        LoadLeaderboard();
    }

    private void LoadLeaderboard()
    {
        // Clean old entries
        foreach (Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }

        FirebaseInitializer.database
            .GetReference("leaderboards")
            .Child(MAP_NAME)
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("❌ Failed to load leaderboard");
                    Debug.LogException(task.Exception);
                    return;
                }

                List<LeaderboardEntry> entries = new();
                Debug.Log("✅ Successfully loaded leaderboard data, processing entries..." + task.Result.ChildrenCount);
                foreach (var child in task.Result.Children)
                {
                    LeaderboardEntry entry = new()
                    {
                        username = child.Child("username").Value.ToString(),
                        hero = child.Child("hero").Value.ToString(),
                        difficulty = child.Child("difficulty").Value.ToString(),
                        score = int.Parse(child.Child("score").Value.ToString()),
                        timeSurvivedSeconds = int.Parse(child.Child("timeSurvivedSeconds").Value.ToString()),
                        timestamp = long.Parse(child.Child("timestamp").Value.ToString())
                    };

                    entries.Add(entry);
                }

                // 🥇 Sort leaderboard
                entries.Sort((a, b) =>
                {
                    int scoreCompare = b.score.CompareTo(a.score);
                    if (scoreCompare != 0) return scoreCompare;

                    int timeCompare = b.timeSurvivedSeconds.CompareTo(a.timeSurvivedSeconds);
                    if (timeCompare != 0) return timeCompare;

                    return a.timestamp.CompareTo(b.timestamp);
                });

                // 🎮 Create UI rows
                for (int i = 0; i < entries.Count; i++)
                {
                    CreatePlacement(entries[i], i + 1);
                }
            });
    }

    private void CreatePlacement(LeaderboardEntry entry, int placement)
    {
        GameObject go = Instantiate(placementGO, content.transform);
        LeaderboardPlacementScript placementScript =
            go.GetComponent<LeaderboardPlacementScript>();

        Color bgColor = Color.white;

        DateTime date = DateTimeOffset.FromUnixTimeMilliseconds(entry.timestamp).LocalDateTime;

        string formattedDate = date.ToString("dd/MM/yyyy HH:mm");

        string timeSurvivedFormatted = $"{entry.timeSurvivedSeconds / 60:00}:{entry.timeSurvivedSeconds % 60:00}";

        Sprite contentImageSprite = placement <= 5 ? topPlaceSprite : otherPlaceSprite;
        Color contentImageColor = Color.white;
        Sprite placementIcon = medals[^1];
        if (placement <= 5)
        {
            placementIcon = medals[placement - 1];
            contentImageColor = colors[placement - 1];
        }
        placementScript.Init(
            contentColor: bgColor,
            placement: placement,
            placementIcon: placementIcon,
            username: entry.username,
            heroName: entry.hero,
            dateOfScore: formattedDate,
            timeSurvived: timeSurvivedFormatted,
            difficulty: entry.difficulty,
            score: entry.score.ToString(),
            contentImageSprite: contentImageSprite,
            contentImageColor: contentImageColor
        );
    }
}