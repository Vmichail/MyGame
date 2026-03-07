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
    [Header("--!!Specific Spawning Enemy Text")]
    [SerializeField] TextMeshProUGUI spawnedSkeletonsText;
    private float elapsedTime;

    private void Update()
    {
        CountTime();
        levelText.text = "Level:" + PlayerStatsManager.Instance.CurrentLevel;
        aliveEnemies.text = "En.Alive:" + GlobalVariables.Instance.aliveEnemies;
        killedEnemies.text = "En.Killed:" + GlobalVariables.Instance.killedEnemies;
        score.text = "Score:" + GlobalVariables.Instance.score;
        //Specific
        spawnedSkeletonsText.text = "SkeletonsSpawned:" + GlobalVariables.Instance.spawnedSkeletons;
        if (DifficultyManager.Instance != null)
        {
            enemyTier.text = "Enemy Tier:" + DifficultyManager.Instance.CurrentTier.ToString();
            enemyHealthMultiplier.text = "HP Mult:" + DifficultyManager.Instance.FinalEnemyHealthMultiplier.ToString("F2");
            enemySpeedMultiplier.text = "Spd Mult:" + DifficultyManager.Instance.FinalEnemySpeedMultiplier.ToString("F2");
            enemyAttackMultiplier.text = "Atk Mult:" + DifficultyManager.Instance.FinalEnemyDamageMultiplier.ToString("F2");
        }
    }

    private void CountTime()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }
}
