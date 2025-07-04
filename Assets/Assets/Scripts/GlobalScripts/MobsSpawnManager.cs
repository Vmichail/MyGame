using UnityEngine;

public class MobsSpawnManagerVariables : MonoBehaviour
{
    public static MobsSpawnManagerVariables Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicates
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Optional: persist across scenes
    }

    public float mobsSpawnTime = 2f;

}
