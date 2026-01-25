using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using System;

public class FirebaseInitializer : MonoBehaviour
{
    public static bool IsReady { get; private set; }
    public static FirebaseAuth auth;
    public static FirebaseDatabase database;
    public static FirebaseApp app;
    public static string PlayerUsername { get; set; }
    public static event Action OnFirebaseReady;
    public static FirebaseInitializer Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        FirebaseApp.CheckAndFixDependenciesAsync()
            .ContinueWithOnMainThread(task =>
            {
                if (task.Result != DependencyStatus.Available)
                {
                    Debug.LogError("❌ Firebase init failed: " + task.Result);
                    return;
                }

                auth = FirebaseAuth.DefaultInstance;
                database = FirebaseDatabase.DefaultInstance;
                if (auth.CurrentUser != null)
                {
                    auth.CurrentUser.ReloadAsync().ContinueWithOnMainThread(reloadTask =>
                        {
                            if (reloadTask.IsFaulted)
                            {
                                Debug.Log("🔴 Cached auth session invalid. Clearing.");

                                auth.SignOut();

                                IsReady = true;
                                OnFirebaseReady?.Invoke();
                                return;
                            }
                            Debug.Log("✅ Auth session is valid");
                            // 👤 Guest vs Registered
                            if (auth.CurrentUser.IsAnonymous)
                            {
                                PlayerUsername = "Guest";
                                IsReady = true;
                                OnFirebaseReady?.Invoke();
                            }
                            else
                            {
                                FetchUsernameAndContinue();
                            }
                        });
                }
                else
                {
                    IsReady = true;
                    OnFirebaseReady?.Invoke();
                }

            });
    }

    public void FetchUsernameAndContinue()
    {
        database
            .GetReference("users")
            .Child(auth.CurrentUser.UserId)
            .Child("username")
            .GetValueAsync()
            .ContinueWithOnMainThread(task =>
            {
                PlayerUsername = task.Result.Exists
                    ? task.Result.Value.ToString()
                    : "Player";
                IsReady = true;
                OnFirebaseReady?.Invoke();
            });
    }
}
