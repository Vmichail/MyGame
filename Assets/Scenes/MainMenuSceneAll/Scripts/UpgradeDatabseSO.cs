using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Database")]
public class UpgradeDatabaseSO : ScriptableObject
{
    public List<UpgradeDataSO> allUpgrades;

    private Dictionary<string, UpgradeDataSO> lookup;

    public void Initialize()
    {
        lookup = new Dictionary<string, UpgradeDataSO>();

        foreach (var upgrade in allUpgrades)
        {
            if (upgrade == null)
            {
                Debug.LogError("Null upgrade found in UpgradeDatabaseSO", this);
                continue;
            }

            if (lookup.ContainsKey(upgrade.upgradeId))
            {
                Debug.LogError($"Duplicate upgradeId: {upgrade.upgradeId}", upgrade);
                continue;
            }

            lookup.Add(upgrade.upgradeId, upgrade);
        }
    }

    public UpgradeDataSO Get(string id)
    {
        if (lookup == null)
            Initialize();

        lookup.TryGetValue(id, out UpgradeDataSO upgrade);
        return upgrade;
    }

    public IEnumerable<UpgradeDataSO> All => allUpgrades;
}