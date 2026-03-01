using UnityEngine;

[CreateAssetMenu(menuName = "MainHero/Character Config")]
public class MainHeroConfig : ScriptableObject
{
    public CharacterSprite characterName;

    [Header("Gameplay")]
    public float rotationFlatFix;
    public bool canAttack;

    [Header("Visuals")]
    public bool enableShadow;
}