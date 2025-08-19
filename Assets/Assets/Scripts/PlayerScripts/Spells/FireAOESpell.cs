using Unity.VisualScripting;
using UnityEngine;

public class FireAOESpell : MonoBehaviour
{
    [SerializeField] SpellData spellData;
    EnemyBaseScript enemyBaseScript;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.TryGetComponent(out enemyBaseScript);
            enemyBaseScript.ReceiveDamage(spellData.damage, 0, 0, Color.white);
        }
    }
}
