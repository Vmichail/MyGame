using TMPro;
using UnityEngine;

public class GenericUIInfo : MonoBehaviour
{
    [Header("--!!Generic Info Text")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI score;
    [Header("--!!Enemy Texts")]
    [SerializeField] TextMeshProUGUI aliveEnemies;
    [SerializeField] TextMeshProUGUI killedEnemies;
    [Header("--!!Enemy Multipliers")]
    [SerializeField] TextMeshProUGUI enemyTier;
    [SerializeField] TextMeshProUGUI enemyHealthMultiplier;
    [SerializeField] TextMeshProUGUI enemySpeedMultiplier;
    [SerializeField] TextMeshProUGUI enemyAttackMultiplier;
    [SerializeField] TextMeshProUGUI enrageMode;
    [Header("--!!Specific Spawning Enemy Text")]
    [SerializeField] TextMeshProUGUI spawnedSkeletonsText;
    private float timer;

    private void Start()
    {
        UpdateAlwaysShownValues();
        if (GlobalVariables.Instance.developerMode)
            UpdateDeveloperInfo();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer = 0f;
            UpdateAlwaysShownValues();
            if (GlobalVariables.Instance.developerMode)
                UpdateDeveloperInfo();
        }
    }

    private void UpdateAlwaysShownValues()
    {
        score.text = "Score:" + GlobalVariables.Instance.score;
        levelText.text = "Level:" + PlayerStatsManager.Instance.CurrentLevel;
        int minutes = Mathf.FloorToInt(GlobalVariables.Instance.gameTime / 60f);
        int seconds = Mathf.FloorToInt(GlobalVariables.Instance.gameTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateDeveloperInfo()
    {
        levelText.text = "Level:" + PlayerStatsManager.Instance.CurrentLevel;
        aliveEnemies.text = "En.Alive:" + GlobalVariables.Instance.aliveEnemies;
        killedEnemies.text = "En.Killed:" + GlobalVariables.Instance.killedEnemies;
        //Specific
        spawnedSkeletonsText.text = "SkeletonsSpawned:" + GlobalVariables.Instance.spawnedSkeletons;
        if (DifficultyManager.Instance != null)
        {
            enemyTier.text = "Enemy Tier:" + DifficultyManager.Instance.CurrentTier.ToString();
            enemyHealthMultiplier.text = "HP Mult:" + DifficultyManager.Instance.FinalEnemyHealthMultiplier.ToString("F2");
            enemySpeedMultiplier.text = "Spd Mult:" + DifficultyManager.Instance.FinalEnemySpeedMultiplier.ToString("F2");
            enemyAttackMultiplier.text = "Atk Mult:" + DifficultyManager.Instance.FinalEnemyDamageMultiplier.ToString("F2");
            enrageMode.text = "Enrage:" + (GlobalVariables.Instance.rangedEnragedMode ? "ON" : "OFF");
        }
    }
}
