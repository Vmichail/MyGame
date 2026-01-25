using UnityEngine;

[CreateAssetMenu(menuName = "Enemies/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    public GlobalVariables.EnemyTypes enemyType;

    [Header("Base Stats")]
    public EnemyStats baseStats;
}