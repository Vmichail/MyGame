using UnityEngine;

public enum DifficultyLevel
{
    Easy,
    Normal,
    Hard,
    Insane
}

public class DifficultyManager : MonoBehaviour
{
    [Header("Time Scaling")]
    [SerializeField] private float minutesPerTier = 2f;

    [SerializeField] private float healthPerTier = 0.15f;
    [SerializeField] private float damagePerTier = 0.12f;
    [SerializeField] private float speedPerTier = 0.05f;

    private float elapsedTime;
    public int CurrentTier { get; private set; }

    [SerializeField] private PlayerScript playerScript;
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel CurrentDifficulty { get; private set; } = DifficultyLevel.Normal;

    [Header("Enemy Stat Multipliers")]
    public float enemyHealthMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;
    public float enemySpeedMultiplier = 1f;

    [Header("Player Stat Multipliers")]
    public float playerDamageMultiplier = 1f;
    public float playerFlatIncrease = 1f;
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
    }

    private void Update()
    {
        if (!startingDifficultySet)
            return;

        elapsedTime += Time.deltaTime;
        CurrentTier = Mathf.FloorToInt(elapsedTime / (minutesPerTier * 60f));
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        CurrentDifficulty = level;

        switch (level)
        {
            case DifficultyLevel.Easy:
                enemyHealthMultiplier = 0.8f;
                enemyDamageMultiplier = 0.7f;
                enemySpeedMultiplier = 0.8f;
                playerDamageMultiplier = 1.2f;
                playerGenericMultiplier = 1.5f;
                playerFlatIncrease = 1;
                break;

            case DifficultyLevel.Normal:
                enemyHealthMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                enemySpeedMultiplier = 1f;
                playerDamageMultiplier = 1f;
                playerGenericMultiplier = 1f;
                playerFlatIncrease = 1;
                break;

            case DifficultyLevel.Hard:
                enemyHealthMultiplier = 1.3f;
                enemyDamageMultiplier = 1.4f;
                enemySpeedMultiplier = 1.1f;
                playerDamageMultiplier = 0.9f;
                playerGenericMultiplier = 0.8f;
                playerFlatIncrease = 0;
                break;

            case DifficultyLevel.Insane:
                enemyHealthMultiplier = 1.8f;
                enemyDamageMultiplier = 2f;
                enemySpeedMultiplier = 1.3f;
                playerDamageMultiplier = 0.8f;
                playerGenericMultiplier = 0.5f;
                playerFlatIncrease = 0;
                break;
        }
        startingDifficultySet = true;
        PlayerStatsManager.Instance.Initialize();
    }

    public float EnemyCoinDropMultiplier = 1f;

    public float FinalEnemyHealthMultiplier =>
    enemyHealthMultiplier * (1f + CurrentTier * healthPerTier);

    public float FinalEnemyDamageMultiplier =>
        enemyDamageMultiplier * (1f + CurrentTier * damagePerTier);

    public float FinalEnemySpeedMultiplier =>
        enemySpeedMultiplier * (1f + CurrentTier * speedPerTier);


}