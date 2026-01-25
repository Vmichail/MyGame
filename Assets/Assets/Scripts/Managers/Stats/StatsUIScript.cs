using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private StatRowUI statRowPrefab;

    private readonly List<StatRowUI> spawnedRows = new();

    private void OnEnable()
    {
        ShowCategory(null);
    }

    public void ShowCategory(PlayerStatCategory? category)
    {

        Clear();

        var manager = PlayerStatsManager.Instance;
        if (manager == null)
            return;
        int index = 0;
        foreach (var kvp in manager.Definitions)
        {
            var definition = kvp.Value;

            if (category != null && definition.category != category.Value)
                continue;

            PlayerStat stat = manager.RuntimeStats.GetStat(definition.type);

            var row = Instantiate(statRowPrefab, contentParent);
            bool isEven = index % 2 == 0;
            row.Setup(definition, stat, isEven);
            row.PlaySpawnAnimation(index * 0.03f);

            spawnedRows.Add(row);
            index++;
        }
    }

    private void Clear()
    {
        foreach (var row in spawnedRows)
            Destroy(row.gameObject);

        spawnedRows.Clear();
    }
    public void ShowAll()
    {
        ShowCategory(null);
    }
    public void ShowOnlyDefence()
    {
        ShowCategory(PlayerStatCategory.Defence);
    }
    public void ShowOnlyAttack()
    {
        ShowCategory(PlayerStatCategory.Attack);
    }

    public void ShowOnlyUtility()
    {
        ShowCategory(PlayerStatCategory.Utility);
    }

    public void ShowOnlySpells()
    {
        ShowCategory(PlayerStatCategory.Spells);
    }
}