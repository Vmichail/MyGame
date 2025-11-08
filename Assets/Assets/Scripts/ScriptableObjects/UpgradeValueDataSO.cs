using UnityEngine;

[CreateAssetMenu(
    fileName = "New Upgrade Value Data",
    menuName = "Upgrades/Upgrade Value Data",
    order = 0)]
public class UpgradeValueDataSO : ScriptableObject
{
    [Header("Upgrade Info")]
    public GlobalVariables.UpgradeCategory upgradeCategory;
    public GlobalVariables.UpgradeCode upgradeCode;

    [Header("Value Settings")]
    public int value;
    public bool percentage;
}