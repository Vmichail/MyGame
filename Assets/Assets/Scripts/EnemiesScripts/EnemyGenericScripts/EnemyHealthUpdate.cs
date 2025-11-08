using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class EnemyHealthUpdate : MonoBehaviour
{
    private TextMeshPro healthText;
    private EnemyBaseScript enemyBaseScript;
    [SerializeField] private bool canShowHealth = true;

    void Start()
    {
        if (!canShowHealth)
        {
            gameObject.SetActive(false);
            return;
        }
        enemyBaseScript = GetComponentInParent<EnemyBaseScript>();
        healthText = GetComponent<TextMeshPro>();
    }


    void Update()
    {
        int roundedHealth = Mathf.RoundToInt(enemyBaseScript.CurrentHealth);
        healthText.SetText(roundedHealth.ToString());
    }
}
