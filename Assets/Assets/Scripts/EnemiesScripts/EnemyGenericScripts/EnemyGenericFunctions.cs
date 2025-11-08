using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public static class EnemyGenericFunctions
{
    static string[] hurtClipNames = { "linaPain1", "linaPain2", "linaPain3" };
    public static float DamagePlayer(float damage)
    {
        AudioManager.Instance.PlayRandomSoundFX(hurtClipNames, Vector2.zero, 1f, 0.9f, 1.1f);
        CinemachineScript.Instance.Shake(2f, 0.25f);
        float armor = GlobalVariables.Instance.playerArmor;
        float damageMultiplier = 1 - ((0.06f * armor) / (1 + 0.06f * Mathf.Abs(armor)));
        float finalDamage = damage * damageMultiplier;

        GlobalVariables.Instance.playerCurrentHealth -= finalDamage;

        // 🔴 Trigger hurt screen flash if the HurtEffect exists in the scene
        if (HurtEffect.Instance != null)
        {
            HurtEffect.Instance.Flash(finalDamage);
        }

        /* Debug.Log($"Incoming: {damage}, Armor: {armor}, Final: {finalDamage}");*/
        return finalDamage;
    }
}