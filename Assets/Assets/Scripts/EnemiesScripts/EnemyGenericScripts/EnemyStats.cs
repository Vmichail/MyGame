using UnityEngine;

[System.Serializable]
public class EnemyStats
{
    [Header("Stats")]
    public float maxHealth;
    public float currentHealth; // optional
    public float damage;
    public float speed;
    public float attackCooldown;
    public bool canBeKnockedBack;
    public float knockbackResistance;

    [Header("Range")]
    public float attackRange;
    public float projectileSpeed;

    [Header("Experience")]
    public float coinDropChance;
    public float healthPotionChance;
    public float manaPotionChance;
    public float greenRubyChance;
    public float redRubyChance;
    public GlobalVariables.CoinDropEnum coinDropEnum;
    public float minExp;
    public float maxExp;

    [Header("Special")]
    public float multipleAttackChance;

    [Header("Audios")]
    public string[] deathSoundClips;
    public string[] attackSoundClips;
    public string[] hurtSounds;

    public EnemyStats(EnemyStats source)
    {
        maxHealth = source.maxHealth;
        damage = source.damage;
        speed = source.speed;
        attackCooldown = source.attackCooldown;
        canBeKnockedBack = source.canBeKnockedBack;
        knockbackResistance = source.knockbackResistance;
        attackRange = source.attackRange;
        projectileSpeed = source.projectileSpeed;
        coinDropChance = source.coinDropChance;
        healthPotionChance = source.healthPotionChance;
        manaPotionChance = source.manaPotionChance;
        greenRubyChance = source.greenRubyChance;
        redRubyChance = source.redRubyChance;
        coinDropEnum = source.coinDropEnum;
        minExp = source.minExp;
        maxExp = source.maxExp;
        multipleAttackChance = source.multipleAttackChance;
        deathSoundClips = source.deathSoundClips;
        attackSoundClips = source.attackSoundClips;
    }
}