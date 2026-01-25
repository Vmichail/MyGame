using UnityEngine;

[CreateAssetMenu(fileName = "NewSpellData", menuName = "Spells/Spell Data", order = 1)]
public class SpellData : ScriptableObject
{
    [Header("Config")]
    public float cooldownTime = 3f;
    public float startingManaCost = 10f;
    public string castSound;

    [Header("Identifier")]
    public GlobalVariables.SpellCode SpellCode;
}