using UnityEngine;
using UnityEngine.UI;

public class MiranaSpecificScript : MonoBehaviour
{
    public static MiranaSpecificScript Instance { get; private set; }
    [Header("Mirana Passives")]
    [SerializeField] private GameObject passive1GO;
    [SerializeField] private Image CooldownMaskImage;
    [SerializeField] private Animator PassiveImageAnimator;

    [Header("Orb References-FX")]
    [SerializeField] private GameObject orb1;
    [SerializeField] private GameObject orb2;
    [SerializeField] private GameObject orb3;
    [SerializeField] private GameObject orb4;
    [SerializeField] private GameObject orb5;
    [SerializeField] private GameObject orb6;
    [SerializeField] private GameObject orb7;
    [SerializeField] private GameObject fx;

    private Animator animator;

    [Header("Skill Timing")]
    [SerializeField] private float skillInterval = 15f;

    private float skillTimer;

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
        animator = GetComponent<Animator>();
        skillTimer = skillInterval; // first cast after interval
    }

    private void OnEnable()
    {
        if (GlobalVariables.Instance.mainMenuScene)
        {
            orb1.SetActive(true);
            orb2.SetActive(true);
            orb3.SetActive(true);
            orb4.SetActive(true);
            orb5.SetActive(true);

        }
        else
        {
            orb1.SetActive(true);
            passive1GO.SetActive(true);
            PassiveImageAnimator.enabled = false;
            orb2.SetActive(false);
            orb3.SetActive(false);
            orb4.SetActive(false);
            orb5.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (passive1GO != null)
            passive1GO.SetActive(false);
        orb1.SetActive(false);
        orb2.SetActive(false);
        orb3.SetActive(false);
        orb4.SetActive(false);
        orb5.SetActive(false);
        orb6.SetActive(false);
        orb7.SetActive(false);
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
            if (PassiveImageAnimator != null)
                PassiveImageAnimator.enabled = true;
            SkillAnimatorTrigger();
        }
    }

    public void SkillAnimatorTrigger()
    {
        animator.SetTrigger("Skill");
    }

    // Called from animation event
    private void CastSkill()
    {
        if (PassiveImageAnimator != null)
            PassiveImageAnimator.enabled = false;
        skillTimer = skillInterval;
        animator.ResetTrigger("Skill");
        orb6.SetActive(true);
        orb7.SetActive(true);
        fx.SetActive(true);
    }

    public void LevelUpCheck(int currentLevel)
    {
        Debug.Log("Checking level up for Mirana, current level: " + currentLevel);
        if (currentLevel >= 5 && !orb2.activeSelf)
        {
            Debug.Log("Activating Orb 2 for Mirana at level " + currentLevel);
            orb1.SetActive(false);
            orb2.SetActive(true);
            orb3.SetActive(true);
        }
        if (currentLevel >= 10 && !orb1.activeSelf)
        {
            orb1.SetActive(true);
        }
        if (currentLevel >= 20 && !orb4.activeSelf)
        {
            orb4.SetActive(true);
        }
        if (currentLevel >= 30 && !orb5.activeSelf)
        {
            orb5.SetActive(true);
        }
    }
}
