using System.Collections.Generic;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;

public static class FirebaseScoreWriter
{
    public static void WriteUserScore(
        string mapName,
        string difficulty,
        int score,
        string hero,
        int timeSurvivedSeconds)
    {
        if (!FirebaseInitializer.IsReady ||
            FirebaseInitializer.auth.CurrentUser == null)
        {
            Debug.LogError("❌ Firebase not ready or user not logged in");
            return;
        }

        string uid = FirebaseInitializer.auth.CurrentUser.UserId;

        DatabaseReference scoreRef =
            FirebaseInitializer.database
                .GetReference("users")
                .Child(uid)
                .Child("scores")
                .Child(mapName);

        Debug.Log($"📍 Reading score at users/{uid}/scores/{mapName}");

        scoreRef.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("❌ Failed to read existing score");
                Debug.LogException(task.Exception);
                return;
            }

            int existingScore = -1;

            if (task.Result.Exists && task.Result.Child("score").Exists)
            {
                int.TryParse(
                    task.Result.Child("score").Value.ToString(),
                    out existingScore
                );
            }

            Debug.Log($"📊 Existing score: {existingScore}, New score: {score}");

            // ✅ Save only if higher OR does not exist
            if (!task.Result.Exists || score > existingScore)
            {
                Dictionary<string, object> data = new()
                {
                    { "map", mapName },
                    { "difficulty", difficulty },
                    { "score", score },
                    { "hero", hero },
                    { "timeSurvivedSeconds", timeSurvivedSeconds },
                    { "timestamp", ServerValue.Timestamp }
                };

                scoreRef.SetValueAsync(data).ContinueWithOnMainThread(writeTask =>
                {
                    if (writeTask.IsFaulted)
                    {
                        Debug.LogError("❌ Failed to save score");
                        Debug.LogException(writeTask.Exception);
                    }
                    else if (writeTask.IsCanceled)
                    {
                        Debug.LogError("❌ Write was canceled");
                    }
                    else
                    {
                        Debug.Log($"✅ Score saved: {mapName} / {score} ({difficulty})");
                        FirebaseLeaderboardWriter.UpdateLeaderboard(
                            mapName,
                            score,
                            hero,
                            difficulty,
                            timeSurvivedSeconds
                        );
                    }
                });
            }
            else
            {
                Debug.Log("ℹ Score NOT saved (existing score is higher or equal)");
            }
        });
    }
}