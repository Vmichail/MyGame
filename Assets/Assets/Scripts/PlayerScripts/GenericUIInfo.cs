using TMPro;
using UnityEngine;

public class GenericUIInfo : MonoBehaviour
{
    [Header("--!!Generic Info Text")]
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI aliveEnemies;
    [SerializeField] TextMeshProUGUI killedEnemies;
    [SerializeField] TextMeshProUGUI score;
    [Header("--!!Specific Spawning Enemy Text")]
    [SerializeField] TextMeshProUGUI spawnedSkeletonsText;
    [SerializeField] TextMeshProUGUI level1BossActiveText;
    private float elapsedTime;

    private void Update()
    {
        CountTime();
        levelText.text = "Level:" + GlobalVariables.Instance.level;
        aliveEnemies.text = "En.Alive:" + GlobalVariables.Instance.aliveEnemies;
        killedEnemies.text = "En.Killed:" + GlobalVariables.Instance.killedEnemies;
        score.text = "Score:" + GlobalVariables.Instance.score;
        //Specific
        spawnedSkeletonsText.text = "SkeletonsSpawned:" + GlobalVariables.Instance.spawnedSkeletons;
        level1BossActiveText.text = "Level1Boss:" + GlobalVariables.Instance.level1BossActive;
    }

    private void CountTime()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"Time:{minutes:00}:{seconds:00}";
    }
}
