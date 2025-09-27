using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellData", menuName = "Spells/Spell Data", order = 1)]
public class SpellData : ScriptableObject
{
    [Header("Config")]
    public float cooldownTime = 3f;
    public float manaCost = 10f;
    public string castSound;
    public bool IsOnCooldown;
    public bool IsActive;

    [Header("Identifier")]
    public GlobalVariables.SpellCode SpellCode;

    public void InitData()
    {
        // Optional: auto-sync default values from GlobalVariables if needed
        if (GlobalVariables.Instance == null)
        {
            Debug.LogWarning("GlobalVariables instance is null. Cannot sync spell data.");
            return;
        }
        ;

        switch (SpellCode)
        {
            case GlobalVariables.SpellCode.FireBlade:
                manaCost = GlobalVariables.Instance.fireBladeManaSpellManaCost;
                cooldownTime = GlobalVariables.Instance.fireBladeCooldown;
                castSound = GlobalVariables.Instance.fireBladeCastSound;
                IsActive = false;
                break;

            case GlobalVariables.SpellCode.RotatingBlades:
                manaCost = GlobalVariables.Instance.orbidBladeSpellManaCost;
                cooldownTime = GlobalVariables.Instance.orbidBladeCooldown;
                castSound = GlobalVariables.Instance.orbidBladeCastSound;
                IsActive = false;
                break;

            case GlobalVariables.SpellCode.Shield:
                manaCost = GlobalVariables.Instance.orbidBladeSpellManaCost;
                cooldownTime = GlobalVariables.Instance.orbidBladeCooldown;
                castSound = GlobalVariables.Instance.orbidBladeCastSound;
                IsActive = false;
                break;
        }
    }
}