using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpellManagerScript : MonoBehaviour
{
    [SerializeField] PlayerScript playerScript;
    private List<SpellData> availableSpells = new();
    [SerializeField] private AllSpellsLogic allSpellsLogic;
    private List<SpellSlotUI> spellSlots = new();

    private Dictionary<GlobalSpellVariables.SpellCodeEnum, bool> cooldownStates = new();

    public bool IsSpellOnCooldown(GlobalSpellVariables.SpellCodeEnum spellCodeEnum)
    {
        return cooldownStates.TryGetValue(spellCodeEnum, out bool isOnCooldown) && isOnCooldown;
    }

    public void SetCooldownState(GlobalSpellVariables.SpellCodeEnum spellCodeEnum, bool isOnCooldown)
    {
        cooldownStates[spellCodeEnum] = isOnCooldown;
    }

    void Update()
    {
        HandleHotkeys();
    }

    void HandleHotkeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) TryCastByIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) TryCastByIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) TryCastByIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) TryCastByIndex(3);
    }

    void TryCastByIndex(int index)
    {
        if (index < availableSpells.Count)
        {
            CastSpell(availableSpells[index]);
        }
    }

    public bool CanCast(SpellData spell)
    {
        return availableSpells.Contains(spell) && (playerScript.ClosestEnemy || spell.canSelftCast) && !IsSpellOnCooldown(spell.SpellCode)
            && GlobalVariables.Instance.playerCurrentMana >= spell.manaCost;
    }

    public void CastSpell(SpellData spell)
    {
        if (!CanCast(spell)) return;

        allSpellsLogic.CastSpellByCode(spell.SpellCode, playerScript.ClosestEnemy);
        GlobalVariables.Instance.playerCurrentMana -= spell.manaCost;
        Debug.Log("PlayerCurrenctMana:" + GlobalVariables.Instance.playerCurrentMana);
        HandleCooldown(spell);
        SetCooldownState(spell.SpellCode, true);
    }

    public void AddSpell(SpellData spell, SpellSlotUI spellSlotUIScript)
    {
        availableSpells.Add(spell);
        spellSlots.Add(spellSlotUIScript);
    }

    private void HandleCooldown(SpellData spellData)
    {
        // Find the SpellSlotUI that matches this spellCode and start cooldown
        foreach (var slot in spellSlots)
        {
            if (slot.SpellCodeEnum == spellData.SpellCode)
            {
                slot.StartCooldown(spellData.cooldown);
                break;
            }
        }
    }
}
