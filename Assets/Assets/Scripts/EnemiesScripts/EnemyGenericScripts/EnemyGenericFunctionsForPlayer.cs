using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenericFunctionsForPlayer : MonoBehaviour
{
    public static EnemyGenericFunctionsForPlayer Instance { get; private set; }
    [SerializeField] private Transform player;

    [Header("VFX")]
    [SerializeField] private GameObject playerWasHitDamage;

    static string[] hurtClipNames = { "linaPain1", "linaPain2", "linaPain3" };

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }


    public void DamagePlayer(float damage)
    {
        if (GlobalVariables.Instance.mainMenuScene)
            return;
        if (GlobalVariables.Instance.playerInvulnerableReasons.Count > 0)
            return;
        AudioManager.Instance.PlayRandomSoundFX(hurtClipNames, Vector2.zero, 1f, 0.9f, 1.1f);
        CinemachineScript.Instance.Shake(2f, 0.25f);
        float armor = PlayerStatsManager.Instance.RuntimeStats.Get(PlayerStatType.Defence_Armor);
        float damageMultiplier = 1 - ((0.06f * armor) / (1 + 0.06f * Mathf.Abs(armor)));
        int finalDamage = Mathf.Max(1, (int)(damage * damageMultiplier));

        PlayerStatsManager.Instance.CurrentHealth -= finalDamage;

        // 🔴 Trigger hurt screen flash if the HurtEffect exists in the scene
        if (HurtEffect.Instance != null)
        {
            HurtEffect.Instance.Flash(finalDamage);
        }

        if (finalDamage >= 1)
        {
            Vector2 randomOffset = new(Random.Range(-0.3f, 0.3f), Random.Range(0.5f, 1.0f));
            Vector2 spawnPosition = (Vector2)player.transform.position + randomOffset;
            GameObject dmgText = Instantiate(playerWasHitDamage, spawnPosition, Quaternion.identity);
            DamageTextScript dt = dmgText.GetComponent<DamageTextScript>();
            dt.SetDamage(damage, false, Color.red);
        }
    }
}