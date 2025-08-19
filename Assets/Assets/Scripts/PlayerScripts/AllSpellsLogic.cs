using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AllSpellsLogic : MonoBehaviour
{
    [SerializeField] private GameObject handOfMidasSpellEffect;
    [SerializeField] private GameObject handOfMidasDeathEffect;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private GameObject fireAOESpellPrefab;
    private SpellData midasSpell;

    private void Start()
    {
        midasSpell = GlobalSpellVariables.Instance.GetSpellByCode(GlobalSpellVariables.SpellCodeEnum.MIDAS);
    }
    public void CastSpellByCode(GlobalSpellVariables.SpellCodeEnum spellCode, GameObject closestEnemy)
    {
        if (GlobalSpellVariables.SpellCodeEnum.MIDAS.Equals(spellCode))
        {
            CastHandOfMidas(closestEnemy);
        }
        else if (GlobalSpellVariables.SpellCodeEnum.FIREAOE.Equals(spellCode))
        {
            CastFireAOE();
        }
    }

    private void CastHandOfMidas(GameObject closestEnemy)
    {
        if (closestEnemy.TryGetComponent(out EnemyBaseScript enemyScript))
        {
            Instantiate(handOfMidasSpellEffect, closestEnemy.transform.position, Quaternion.identity);
            enemyScript.ReceiveDamage(midasSpell.damage, 0, 0, Color.yellow);
            if (enemyScript.CurrentHealth <= midasSpell.damage)
            {
                Instantiate(handOfMidasDeathEffect, closestEnemy.transform.position, Quaternion.identity);
                GlobalVariables.Instance.coinsCollected += GlobalVariables.Instance.yellowCoinValue * 5;
            }
        }

    }

    private void CastFireAOE()
    {
        GameObject aoe = Instantiate(fireAOESpellPrefab, transform.position, Quaternion.identity);
        aoe.transform.SetParent(transform);
    }


}
