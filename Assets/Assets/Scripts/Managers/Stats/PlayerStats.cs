using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerStats
{
    private Dictionary<PlayerStatType, PlayerStat> stats = new();

    public PlayerStats() { }

    public PlayerStats(PlayerStats other)
    {
        foreach (var kv in other.stats)
            stats[kv.Key] = new PlayerStat(kv.Value);
    }

    public void Register(PlayerStatType type, float baseValue)
    {
        stats[type] = new PlayerStat(baseValue);
    }

    public float Get(PlayerStatType type)
    {
        return stats[type].Value;
    }

    public float GetBase(PlayerStatType type)
    {
        return stats[type].baseValue;
    }

    public PlayerStat GetStat(PlayerStatType type)
    {
        return stats[type];
    }

    public void AddFlat(PlayerStatType type, float value)
    {
        stats[type].flatBonus += value;
    }

    public void AddPermanentUpdate(PlayerStatType type, float value)
    {
        //Debug.Log($"Permanent Upgrade {type} Added with value {value}");
        stats[type].permanentUpgrade += value;
    }

    public void RemovePermanentUpdate(PlayerStatType type, float value)
    {
        Console.Write($"Permanent Upgrade {type} Removed with value {value}");
        stats[type].permanentUpgrade -= value;
    }

    public void AddMultiplier(PlayerStatType type, float multiplier)
    {
        stats[type].multiplier += multiplier;
    }

    public void AddLevelValue(PlayerStatType type, float value)
    {
        stats[type].levelBonus += value;
    }

    public Dictionary<PlayerStatType, PlayerStat> GetAll()
    {
        return stats;
    }
}
