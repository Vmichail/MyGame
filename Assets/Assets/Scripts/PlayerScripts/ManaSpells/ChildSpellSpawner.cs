using UnityEngine;

public class ChildSpellSpawner : MonoBehaviour
{
    public GameObject childSpellPrefab;
    public float childSpellSpeed = 5f;

    private Vector2[] directions = new Vector2[]
{
        Vector2.up,                            // 0°   (Up)
        new Vector2(1, 2).normalized,          // 22.5°
        new Vector2(1, 1).normalized,          // 45°  (Top-Right)
        new Vector2(2, 1).normalized,          // 67.5°
        Vector2.right,                         // 90°  (Right)
        new Vector2(2, -1).normalized,         // 112.5°
        new Vector2(1, -1).normalized,         // 135° (Bottom-Right)
        new Vector2(1, -2).normalized,         // 157.5°
        Vector2.down,                          // 180° (Down)
        new Vector2(-1, -2).normalized,        // 202.5°
        new Vector2(-1, -1).normalized,        // 225° (Bottom-Left)
        new Vector2(-2, -1).normalized,        // 247.5°
        Vector2.left,                          // 270° (Left)
        new Vector2(-2, 1).normalized,         // 292.5°
        new Vector2(-1, 1).normalized,         // 315° (Top-Left)
        new Vector2(-1, 2).normalized          // 337.5°
};


    public void SpawnChildSpells(Vector2 spawnPosition)
    {
        foreach (Vector2 dir in directions)
        {
            GameObject childSpell = Instantiate(childSpellPrefab, spawnPosition, Quaternion.identity);

            Rigidbody2D rb = childSpell.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = dir * childSpellSpeed;
            }
        }
    }
}