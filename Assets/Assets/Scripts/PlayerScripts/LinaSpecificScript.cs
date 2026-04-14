using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class LinaSpecificScript : MonoBehaviour
{
    public static LinaSpecificScript Instance { get; private set; }
    [Header("Lina Passives")]
    [SerializeField] private GameObject passive1GO;
    [SerializeField] private Image CooldownMaskImage;
    [SerializeField] private GameObject fireTornadoPrefab; // 20% damage bonus

    [Header("FX")]
    [SerializeField] private GameObject fx;

    [Header("Skill Timing")]
    [SerializeField] private float skillInterval = 10f;

    private float skillTimer;
    Vector2[] selectedDirections = { Vector2.right, Vector2.left };
    Vector2[] directions1 = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    Vector2[] directions2 = {Vector2.up, Vector2.right, Vector2.down, Vector2.left,
            (Vector2.up + Vector2.right).normalized, (Vector2.up + Vector2.left).normalized, (Vector2.down + Vector2.right).normalized, (Vector2.down + Vector2.left).normalized };

    private void Awake()
    {
        // Singleton enforcement
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        skillTimer = skillInterval; // first cast after interval
    }

    private void OnEnable()
    {
        if (GlobalVariables.Instance.mainMenuScene)
        {
            selectedDirections = directions2;
            skillInterval = 7.5f;
        }
        else
        {
            passive1GO.SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (passive1GO != null)
            passive1GO.SetActive(false);
    }

    private void Update()
    {
        skillTimer -= Time.deltaTime;

        if (CooldownMaskImage != null)
        {
            CooldownMaskImage.fillAmount = Mathf.Clamp01(skillTimer / skillInterval);
        }

        if (skillTimer <= 0f)
        {
            CastSkill();
        }
    }

    // Called from animation event
    private void CastSkill()
    {
        skillTimer = skillInterval;
        fx.SetActive(true);
        AudioManager.Instance.PlaySoundFX("FireTornadoCast", transform.position, 0.7f, 1f, 1f);
        foreach (Vector2 dir in selectedDirections)
        {
            GameObject newSpell = PoolManager.Instance.Get(fireTornadoPrefab, transform.position, Quaternion.identity, PoolCategory.Player);

            if (newSpell.TryGetComponent<PlayerSpellBaseScript>(out var playerSpellBaseScript))
            {
                playerSpellBaseScript.SetVelocity(dir, false);
            }
            else
            {
                Debug.LogWarning("FireTornado prefab is missing PlayerSpellBaseScript!");
            }
        }
    }

    public void LevelUpCheck(int currentLevel)
    {
        PlayerStatsManager stats = PlayerStatsManager.Instance;
        if (currentLevel == 5)
        {
            selectedDirections = directions1;
        }
        else if (currentLevel == 15)
        {
            selectedDirections = directions2;
        }
        if (currentLevel > 10 && skillInterval >= 5f)
        {
            skillInterval -= 1f;
        }
        stats.IncreaseMaxHealthFromLevels(2);
        stats.IncreaseMaxManaFromLevels(2);
        if (stats.CurrentLevel % 5 == 0)
        {
            stats.RuntimeStats.AddLevelValue(PlayerStatType.Attack_Attack, 1);
        }

    }
}
