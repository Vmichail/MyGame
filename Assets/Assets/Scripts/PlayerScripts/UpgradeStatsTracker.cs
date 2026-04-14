using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeStackTracker : MonoBehaviour
{
    public static UpgradeStackTracker Instance { get; private set; }

    private Dictionary<PlayerStatType, int> purchaseCounts = new();

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void Register(PlayerStatType type)
    {
        if (!purchaseCounts.ContainsKey(type))
            purchaseCounts[type] = 0;
        purchaseCounts[type]++;
    }

    public int GetCount(PlayerStatType type)
    {
        return purchaseCounts.TryGetValue(type, out int count) ? count : 0;
    }

    public bool IsMaxed(UpgradeChoice upgrade)
    {
        if (upgrade.maxStack == 0) return false;
        return GetCount(upgrade.UpgradeCode) >= upgrade.maxStack;
    }

    public string GetStackLabel(UpgradeChoice upgrade)
    {
        int current = GetCount(upgrade.UpgradeCode);
        string max = upgrade.maxStack == 0 ? "∞" : upgrade.maxStack.ToString();
        return $"{current}/{max}";
    }

    public void Reset()
    {
        purchaseCounts.Clear();
    }
}