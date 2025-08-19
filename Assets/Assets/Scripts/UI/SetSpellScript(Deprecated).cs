using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SetSpellScript : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Image iconImage;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI cooldownText;
    private SpellData selectedSpell;


    public void SetSpell(SpellData spell)
    {
        titleText.text = spell.title;
        iconImage.sprite = spell.icon;
        descriptionText.text = spell.description;
        damageText.text = "Damage: " + spell.damage;
        cooldownText.text = "Cooldown: " + spell.cooldown + "s";
        selectedSpell = spell;
    }

    public SpellData GetSpell()
    {
        return selectedSpell;
    }
}


