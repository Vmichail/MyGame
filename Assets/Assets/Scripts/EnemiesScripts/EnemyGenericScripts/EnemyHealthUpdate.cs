using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class EnemyHealthUpdate : MonoBehaviour
{
    private TextMeshPro healthText;
    private EnemyBaseScript enemyBaseScript;

    void Start()
    {
        enemyBaseScript = GetComponentInParent<EnemyBaseScript>();
        healthText = GetComponent<TextMeshPro>();
    }


    void Update()
    {
        int roundedHealth = Mathf.RoundToInt(enemyBaseScript.CurrentHealth);
        healthText.SetText(roundedHealth.ToString());
    }
}
