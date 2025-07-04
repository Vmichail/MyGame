using UnityEngine;

[CreateAssetMenu(fileName = "EnemyTypesScripableObject", menuName = "Scriptable Objects/EnemyTypesScripableObject")]
public class EnemyTypesScriptableObject : ScriptableObject
{
    public GlobalVariables.EnemyTypes type;
    public GameObject prefab;
}
