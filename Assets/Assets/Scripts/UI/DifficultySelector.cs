using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    [SerializeField] private PlayerScript playerScript;
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel CurrentDifficulty { get; private set; } = DifficultyLevel.Normal;

    [Header("Enemy Stat Multipliers")]
    public float enemyHealthMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;
    public float enemySpeedMultiplier = 1f;

    [Header("Player Stat Multipliers")]
    public float playerDamageMultiplier = 1f;
    public float playerGenericMultiplier = 1f;

    public bool startingDifficultySet = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        CurrentDifficulty = level;

        switch (level)
        {
            case DifficultyLevel.Easy:
                enemyHealthMultiplier = 0.8f;
                enemyDamageMultiplier = 0.7f;
                enemySpeedMultiplier = 0.9f;
                playerDamageMultiplier = 1.2f;
                playerGenericMultiplier = 1.5f;
                break;

            case DifficultyLevel.Normal:
                enemyHealthMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                enemySpeedMultiplier = 1f;
                playerDamageMultiplier = 1f;
                playerGenericMultiplier = 1f;
                break;

            case DifficultyLevel.Hard:
                enemyHealthMultiplier = 1.3f;
                enemyDamageMultiplier = 1.4f;
                enemySpeedMultiplier = 1.1f;
                playerDamageMultiplier = 0.9f;
                playerGenericMultiplier = 0.8f;
                break;

            case DifficultyLevel.Insane:
                enemyHealthMultiplier = 1.8f;
                enemyDamageMultiplier = 2f;
                enemySpeedMultiplier = 1.3f;
                playerDamageMultiplier = 0.8f;
                playerGenericMultiplier = 0.5f;
                break;
        }
        startingDifficultySet = true;
        playerScript.DifficultyScaler();
    }


    public enum DifficultyLevel
    {
        Easy,
        Normal,
        Hard,
        Insane
    }
}