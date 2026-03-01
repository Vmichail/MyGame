using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Upgrade Data")]
public class UpgradeDataSO : ScriptableObject
{
    [Header("ID (DO NOT CHANGE AFTER RELEASE)")]
    public string upgradeId;

    [Header("UI Text")]
    public string title;
    [TextArea] public string description;

    [Header("Economy")]
    public int price;

    [Header("Upgrade Info")]
    public PlayerStatType playerStatType;
    public float upgradeAmount;

    [Header("Optional")]
    public Sprite icon;

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(upgradeId))
            Debug.LogError("Upgrade ID cannot be empty", this);
    }
#endif
}
