using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Stats/Player Base Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [System.Serializable]
    public class StatEntry
    {
        public PlayerStatType type;
        public float baseValue;
    }

    public List<StatEntry> stats = new();
}