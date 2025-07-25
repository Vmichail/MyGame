using UnityEngine;

public class HealthRegen : MonoBehaviour
{
    private float regenTimer;

    void Update()
    {
        if (GlobalVariables.Instance.regenIsActive)
            RegenerateHealth();
    }

    public void RegenerateHealth()
    {
        regenTimer += Time.deltaTime;
        if (regenTimer >= GlobalVariables.Instance.playerHealthRegenInterval)
        {
            GlobalVariables.Instance.playerCurrentHealth = Mathf.Min(GlobalVariables.Instance.playerCurrentHealth + GlobalVariables.Instance.playerHealthRegen, GlobalVariables.Instance.playerMaxHealth);
            regenTimer = 0f;
        }
    }
}