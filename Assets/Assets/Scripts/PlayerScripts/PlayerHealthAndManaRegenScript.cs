using UnityEngine;

public class PlayerHealthAndManaRegen : MonoBehaviour
{
    private float healthRegenTimer;
    private float manaRegenTimer;

    void Update()
    {
        if (GlobalVariables.Instance.healthRegenIsActive)
            RegenerateHealth();
        if (GlobalVariables.Instance.healthRegenIsActive)
            RegenerateMana();
    }

    public void RegenerateHealth()
    {
        healthRegenTimer += Time.deltaTime;
        if (healthRegenTimer >= GlobalVariables.Instance.playerHealthRegenInterval)
        {
            GlobalVariables.Instance.playerCurrentHealth = Mathf.Min(GlobalVariables.Instance.playerCurrentHealth + GlobalVariables.Instance.playerHealthRegen, GlobalVariables.Instance.playerMaxHealth);
            healthRegenTimer = 0f;
        }
    }

    public void RegenerateMana()
    {
        manaRegenTimer += Time.deltaTime;
        if (manaRegenTimer >= GlobalVariables.Instance.playerManaRegenInterval)
        {
            GlobalVariables.Instance.playerCurrentMana = Mathf.Min(GlobalVariables.Instance.playerCurrentMana + GlobalVariables.Instance.playerManaRegen, GlobalVariables.Instance.playerMaxMana);
            manaRegenTimer = 0f;
        }
    }
}