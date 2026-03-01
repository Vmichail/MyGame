using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class FirebaseLeaderboardWriter
{
    public static void UpdateLeaderboard(
        string mapName,
        int score,
        string hero,
        string difficulty,
        int timeSurvivedSeconds)
    {
        if (!FirebaseInitializer.IsReady ||
            FirebaseInitializer.auth.CurrentUser == null)
        {
            Debug.LogError("Firebase not ready or user not logged in");
            return;
        }

        string uid = FirebaseInitializer.auth.CurrentUser.UserId;
        string username = FirebaseInitializer.PlayerUsername;

        DatabaseReference leaderboardRef =
            FirebaseInitializer.database
                .GetReference("leaderboards")
                .Child(mapName)
                .Child(uid);

        var data = new Dictionary<string, object>
        {
            { "username", username },
            { "score", score },
            { "hero", hero },
            { "difficulty", difficulty },
            { "timeSurvivedSeconds", timeSurvivedSeconds },
            { "timestamp", ServerValue.Timestamp }
        };

        leaderboardRef.SetValueAsync(data).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("❌ Failed to update leaderboard");
                Debug.LogException(task.Exception);
            }
            else
            {
                Debug.Log($"🏆 Leaderboard updated: {mapName} / {username} / {score}");
            }
        });
    }
}