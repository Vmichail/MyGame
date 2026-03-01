using UnityEngine;
using UnityEngine.TextCore.Text;

public class HealEffectSelector : MonoBehaviour
{
    public static HealEffectSelector Instance { get; private set; }

    [Header("Effects")]
    [SerializeField] private GameObject passiveHealEffect;
    [SerializeField] private GameObject passiveManaEffect;
    [SerializeField] private GameObject healthPotionEffect;
    [SerializeField] private GameObject manaPotionEffect;
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject whiteEffect;

    public enum PlayerHealEffectType
    {
        PassiveHeal,
        PassiveMana,
        HealthPotion,
        ManaPotion,
        Fire,
        White
    }

    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnEnable()
    {
        if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.LinaSprite.ToString()))
        {
            transform.localPosition = new Vector2(-0.18f, -0.09f);
        }
        else if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.MiranaSprite.ToString()))
        {
            transform.localPosition = new Vector2(0.031f, -0.12f);
        }
    }

    public static void SelectHealEffect(PlayerHealEffectType effectType)
    {
        if (Instance == null)
        {
            Debug.LogWarning("HealEffectSelector.Instance is null!");
            return;
        }

        switch (effectType)
        {
            case PlayerHealEffectType.PassiveHeal:
                Instance.passiveHealEffect.SetActive(true);
                break;
            case PlayerHealEffectType.PassiveMana:
                Instance.passiveManaEffect.SetActive(true);
                break;
            case PlayerHealEffectType.HealthPotion:
                Instance.healthPotionEffect.SetActive(true);
                break;
            case PlayerHealEffectType.ManaPotion:
                Instance.manaPotionEffect.SetActive(true);
                break;
            case PlayerHealEffectType.Fire:
                Instance.fireEffect.SetActive(true);
                break;
            case PlayerHealEffectType.White:
                Instance.whiteEffect.SetActive(true);
                break;
            default:
                Debug.LogWarning("Unknown effect type: " + effectType);
                break;
        }
    }
}