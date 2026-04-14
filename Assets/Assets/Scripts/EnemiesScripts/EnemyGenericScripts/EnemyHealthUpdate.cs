using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class EnemyHealthUpdate : MonoBehaviour
{
    private TextMeshPro healthText;
    private EnemyBaseScript enemyBaseScript;
    [SerializeField] private bool isBoss;

    private void Awake()
    {
        healthText = GetComponent<TextMeshPro>();
        enemyBaseScript = GetComponentInParent<EnemyBaseScript>();
    }

    private void OnEnable()
    {
        GlobalVariables.Instance.OnShowAllEnemiesHealthChanged += OnHealthVisibilityChanged;
        OnHealthVisibilityChanged(GlobalVariables.Instance.showAllEnemiesHealth);
    }

    private void OnDisable()
    {
        GlobalVariables.Instance.OnShowAllEnemiesHealthChanged -= OnHealthVisibilityChanged;
    }

    private void OnHealthVisibilityChanged(bool show)
    {
        if (GlobalVariables.Instance.mainMenuScene)
        {
            healthText.enabled = false;
            return;
        }
        if (isBoss)
        {
            healthText.enabled = true;
            return;
        }

        healthText.enabled = show;
        if (show)
        {
            int roundedHealth = Mathf.RoundToInt(enemyBaseScript.CurrentHealth);
            healthText.SetText(roundedHealth.ToString());
        }
    }

    public void UpdateHealth(float currentHealth)
    {
        if (healthText == null || !healthText.enabled) return;
        int roundedHealth = Mathf.RoundToInt(currentHealth);
        healthText.SetText(roundedHealth.ToString());
    }
}