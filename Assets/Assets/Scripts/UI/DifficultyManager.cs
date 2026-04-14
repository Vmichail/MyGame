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
    [SerializeField] private float secondsPerTier = 2f;
    [Header("Easy")]
    [SerializeField] private float easyHealthPerTier = 0.2f;
    [SerializeField] private float easyDamagePerTier = 0.2f;
    [SerializeField] private float easySpeedPerTier = 0.02f;
    [SerializeField] private int easyEnrageTimer = 600;
    [Header("Normal")]
    [SerializeField] private float normalHealthPerTier = 0.2f;
    [SerializeField] private float normalDamagePerTier = 0.2f;
    [SerializeField] private float normalSpeedPerTier = 0.02f;
    [SerializeField] private int normalEnrageTimer = 400;
    [Header("Hard")]
    [SerializeField] private float hardHealthPerTier = 0.2f;
    [SerializeField] private float hardDamagePerTier = 0.2f;
    [SerializeField] private float hardSpeedPerTier = 0.02f;
    [SerializeField] private int hardEnrageTimer = 120;
    [Header("Insane")]
    [SerializeField] private float insaneHealthPerTier = 0.2f;
    [SerializeField] private float insaneDamagePerTier = 0.2f;
    [SerializeField] private float insaneSpeedPerTier = 0.02f;
    [SerializeField] private int insaneEnrageTimer = 0;

    private float healthPerTier;
    private float damagePerTier;
    private float speedPerTier;
    private int enrageTimer = 0;

    public int EnrageTimer => enrageTimer;

    private float elapsedTime;
    public int CurrentTier { get; private set; }

    [SerializeField] private PlayerScript playerScript;
    public static DifficultyManager Instance { get; private set; }

    public DifficultyLevel CurrentDifficulty { get; private set; } = DifficultyLevel.Normal;

    [Header("Enemy Stat Multipliers")]
    public float enemyHealthMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;
    public float enemySpeedMultiplier = 1f;
    public int scoreMultiplier = 1;

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
        CurrentTier = Mathf.FloorToInt(elapsedTime / secondsPerTier);
    }

    public void SetDifficulty(DifficultyLevel level)
    {
        CurrentDifficulty = level;
        GlobalVariables.Instance.rangedEnragedMode = false;

        switch (level)
        {
            case DifficultyLevel.Easy:
                enemyHealthMultiplier = 0.4f;
                enemyDamageMultiplier = 0.7f;
                enemySpeedMultiplier = 0.7f;
                playerDamageMultiplier = 1.2f;
                playerGenericMultiplier = 1.5f;
                playerFlatIncrease = 1;
                scoreMultiplier = 1;
                enrageTimer = easyEnrageTimer;
                healthPerTier = easyHealthPerTier;
                damagePerTier = easyDamagePerTier;
                speedPerTier = easySpeedPerTier;
                break;

            case DifficultyLevel.Normal:
                enemyHealthMultiplier = 1f;
                enemyDamageMultiplier = 1f;
                enemySpeedMultiplier = 1f;
                playerDamageMultiplier = 1f;
                playerGenericMultiplier = 1f;
                playerFlatIncrease = 1;
                scoreMultiplier = 2;
                enrageTimer = normalEnrageTimer;
                healthPerTier = normalHealthPerTier;
                damagePerTier = normalDamagePerTier;
                speedPerTier = normalSpeedPerTier;
                break;

            case DifficultyLevel.Hard:
                enemyHealthMultiplier = 1.3f;
                enemyDamageMultiplier = 1.4f;
                enemySpeedMultiplier = 1.1f;
                playerDamageMultiplier = 0.9f;
                playerGenericMultiplier = 0.8f;
                playerFlatIncrease = 0;
                scoreMultiplier = 5;
                enrageTimer = hardEnrageTimer;
                healthPerTier = hardHealthPerTier;
                damagePerTier = hardDamagePerTier;
                speedPerTier = hardSpeedPerTier;
                break;

            case DifficultyLevel.Insane:
                enemyHealthMultiplier = 1.8f;
                enemyDamageMultiplier = 2f;
                enemySpeedMultiplier = 1.3f;
                playerDamageMultiplier = 0.8f;
                playerGenericMultiplier = 0.5f;
                playerFlatIncrease = 0;
                scoreMultiplier = 10;
                enrageTimer = insaneEnrageTimer;
                healthPerTier = insaneHealthPerTier;
                damagePerTier = insaneDamagePerTier;
                speedPerTier = insaneSpeedPerTier;
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