using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpellVariables : MonoBehaviour
{
    public static GlobalSpellVariables Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public List<SpellData> allSpells = new();

    public List<SpellCodeEnum> learnedSpells = new();

    public SpellData GetSpellByCode(GlobalSpellVariables.SpellCodeEnum code)
    {
        foreach (SpellData spell in allSpells)
        {
            if (spell.SpellCode == code)
                return spell;
        }
        Debug.LogWarning($"Spell with code {code} not found!");
        return null;
    }

    public enum SpellCodeEnum
    {
        MIDAS,
        FIREAOE,
        WHATEVER,
    }
}
