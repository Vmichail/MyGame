using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Spell")]
public class SpellData : ScriptableObject
{
    public string title;
    public Sprite icon;
    public string description;
    public int damage;
    public int manaCost;
    public int cooldown;
    public bool relearn;
    public bool canSelftCast;
    public Sprite smallIcon;
    [SerializeField]
    private GlobalSpellVariables.SpellCodeEnum spellCode;

    public GlobalSpellVariables.SpellCodeEnum SpellCode => spellCode;
}