using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeLoadManager : MonoBehaviour
{
    public static UpgradeLoadManager Instance;
    public static event System.Action OnUpgradesChanged;

    public IReadOnlyCollection<string> GetPurchasedUpgrades()
    {
        return SaveSystem.Data.purchasedUpgrades;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsPurchased(string upgradeId)
    {
        return SaveSystem.Data.purchasedUpgrades.Contains(upgradeId);
    }

    public void Purchase(UpgradeDataSO upgrade)
    {
        if (SaveSystem.Data.purchasedUpgrades.Contains(upgrade.upgradeId))
        {
            Debug.LogError($"Upgrade {upgrade.upgradeId} is already found on purchased upgrades.");
            return;
        }
        SaveSystem.Data.purchasedUpgrades.Add(upgrade.upgradeId);
        SaveSystem.Save();
        OnUpgradesChanged?.Invoke();
    }

    public void ClearAllPurchases()
    {
        SaveSystem.Data.purchasedUpgrades.Clear();
        SaveSystem.Save();
        OnUpgradesChanged?.Invoke();
    }
}