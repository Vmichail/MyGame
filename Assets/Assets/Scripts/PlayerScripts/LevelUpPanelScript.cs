using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class LevelUpPanelScript : MonoBehaviour
{
    [Header("Cats")]
    [SerializeField] private Transform catsGOParent;
    [Header("Upgrade Choices")]
    [SerializeField] private Transform upgradeChoicesParentTransform;
    [SerializeField] private GameObject wholeLevelUpPanel;
    [SerializeField] private List<UpgradeChoice> upgradeChoices = new();
    [SerializeField] int numberOfOptions = 5;
    [SerializeField] GameObject upgradeOptionPrefab;
    private int upgradeCost = 0;
    private List<UpgradeChoice> randomChoices = new();
    private List<UpgradeChoice> allSpells = new();
    [SerializeField] private Animator buffAnimator;
    private List<SetUpgradeScript> upgradeScripts = new();
    [SerializeField] private GameObject refreshButtonGO;
    private Vector2 refreshButtonInitialPosition;
    [SerializeField] private TextMeshProUGUI refreshButtonTextValue;
    private int currentRefreshes;
    public bool HealthCost = false;
    private Image levelUpPanelImage;
    private Color normalColor;
    private string[] levelUpSounds = new[] { "levelup1", "levelup2", "levelup3" };
    private int maxRefreshes = 0;
    private void Awake()
    {
        allSpells = upgradeChoices.Where(uc => uc.UpgradeCategory == PlayerStatCategory.Spells).ToList();
        levelUpPanelImage = wholeLevelUpPanel.GetComponent<Image>();
        normalColor = levelUpPanelImage.color;
        refreshButtonInitialPosition = refreshButtonGO.transform.localPosition;
    }

    private void Start()
    {
        currentRefreshes = 0;
        if (maxRefreshes == 0)
        {
            PlayerStat playerStat = PlayerStatsManager.Instance.RuntimeStats.GetStat(PlayerStatType.Reroll);
            maxRefreshes = Mathf.FloorToInt(playerStat.Value);
            refreshButtonTextValue.text = maxRefreshes.ToString();
            UpdateRefreshTextColor();
        }
    }


    public void RefreshChoices(bool resetCost)
    {
        levelUpPanelImage.color = normalColor;
        if (resetCost)
        {
            upgradeCost = 0;
        }
        randomChoices = null;

        upgradeScripts.Clear();
        foreach (Transform child in upgradeChoicesParentTransform)
        {
            Destroy(child.gameObject);
        }
        randomChoices = GetRandomChoices(numberOfOptions);
        int optionCount = Mathf.Min(numberOfOptions, randomChoices.Count);
        for (int i = 0; i < optionCount; i++)
        {
            UpgradeChoice randomFirstUpgrade = randomChoices[i];
            bool isLocked = false;
            GameObject upgradeChoice = Instantiate(upgradeOptionPrefab, upgradeChoicesParentTransform);
            upgradeChoice.name = "UpgradeOption" + i;
            if (upgradeChoice.TryGetComponent(out SetUpgradeScript setUpgradeScript))
            {
                upgradeScripts.Add(setUpgradeScript);
                if (i == 3 && !SaveSystem.Data.purchasedUpgrades.Contains("1UpgradeOption"))
                {
                    isLocked = true;
                }
                if (i == 4 && !SaveSystem.Data.purchasedUpgrades.Contains("2UpgradeOption"))
                {
                    isLocked = true;
                }
                List<UpgradeChoice> bonusUpgrades = GetBonusBasedOnLuck(randomFirstUpgrade.UpgradeCategory);
                setUpgradeScript.SetUpgradeChoice(randomFirstUpgrade, upgradeCost, isLocked, bonusUpgrades);
                //Button
                if (upgradeChoice.TryGetComponent<Button>(out var button))
                {
                    if (!isLocked)
                        button.onClick.AddListener(() => BuyOption(randomFirstUpgrade, button.gameObject, setUpgradeScript, bonusUpgrades));
                }
                if (i == 0)
                {
                    SetNavigationGO(upgradeChoice);
                }
            }

        }
    }
    public List<UpgradeChoice> GetRandomChoices(int count)
    {
        return upgradeChoices
        .Where(uc => !UpgradeStackTracker.Instance.IsMaxed(uc))
        .OrderBy(x => Random.value)
        .Take(count)
        .ToList();
    }

    //public List<UpgradeChoice> GetOnlySpells(int count)
    //{
    //    return allSpells
    //        .OrderBy(x => Random.value)  // Random.value gives a float between 0–1
    //        .Take(count)
    //        .ToList();
    //}
    private void OnEnable()
    {
        CursorManagerScript.MouseIsActive = false;
        GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);
        ShowCatUi();

        AudioManager.Instance.PlayRandomSoundFX(levelUpSounds, transform.position, 1f, 1f, 1f);

        // Animate panel
        wholeLevelUpPanel.transform.localScale = Vector3.zero;

        if (!wholeLevelUpPanel.TryGetComponent<CanvasGroup>(out var cg))
            cg = wholeLevelUpPanel.AddComponent<CanvasGroup>();

        cg.alpha = 0;

        // Panel scale animation
        wholeLevelUpPanel.transform
            .DOScale(Vector3.one, 0.5f)
            .SetEase(Ease.OutBack)
            .SetUpdate(true);

        // Fade animation
        cg.DOFade(1f, 0.4f)
          .SetUpdate(true);

        if (currentRefreshes < maxRefreshes)
        {
            refreshButtonGO.SetActive(true);
            refreshButtonGO.transform.localScale = Vector3.zero;
            refreshButtonGO.transform.localPosition = refreshButtonInitialPosition;

            refreshButtonGO.transform
                .DOScale(Vector3.one, 0.3f)
                .SetEase(Ease.OutBack)
                .SetUpdate(true);

        }
        RefreshChoices(true);
    }

    public void BuyOption(UpgradeChoice chosenUpgrade, GameObject buttonGO, SetUpgradeScript setUpgradeScript, List<UpgradeChoice> bonusChoices)
    {
        if (CurrencyManager.instance.Gold >= upgradeCost)
        {
            if (buttonGO.TryGetComponent(out Button button))
            {
                button.interactable = false;
            }
            CurrencyManager.instance.Add(-upgradeCost);
            TriggerBuff();
            randomChoices.Remove(chosenUpgrade);
            upgradeCost += 10;
            UpdatePrices();
            setUpgradeScript.IsClicked = true;
            AudioManager.Instance.PlaySoundFX("cardChooseSound", transform.position, 1f, 0.75f, 1.25f);
            AnimatedDeletion(buttonGO);
            HeroUpgrades.Instance.UpgradeHero(chosenUpgrade);
            UpgradeStackTracker.Instance.Register(chosenUpgrade.UpgradeCode);
            foreach (UpgradeChoice bonus in bonusChoices)
            {
                HeroUpgrades.Instance.UpgradeHero(bonus);
                UpgradeStackTracker.Instance.Register(bonus.UpgradeCode);
            }
            Continue();
        }
        else
        {
            buttonGO.transform.DOKill();
            buttonGO.transform
                .DOScale(Vector3.one * 1.05f, 0.5f)
                .SetEase(Ease.InOutSine)
                .SetLoops(2, LoopType.Yoyo)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    buttonGO.transform.localScale = Vector3.one;
                });
        }
    }
    private void UpdatePrices(int cost = -1)
    {
        if (cost == -1)
        {
            foreach (SetUpgradeScript upgradeScript in upgradeScripts)
            {
                upgradeScript.SetPrice(upgradeCost);
            }
        }
        else
        {
            foreach (SetUpgradeScript upgradeScript in upgradeScripts)
            {
                upgradeScript.SetPrice(cost);
            }
        }
    }



    public void Continue()
    {
        AudioManager.Instance.PlayRandomSoundFX(GlobalVariables.Instance.linaAnnouncementsClips, transform.position, 1f, 1f, 1f);
        AudioManager.Instance.PlaySoundFX("buttonClickSound", transform.position, 1f, 0.75f, 1.25f);
        wholeLevelUpPanel.SetActive(false);
        GlobalVariables.Instance.UnPauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);
        HealthCost = false;
    }


    public void TriggerBuff()
    {
        if (buffAnimator != null)
        {
            buffAnimator.ResetTrigger("PlayBuff");
            buffAnimator.SetTrigger("PlayBuff");
        }
        else
        {
            Debug.LogWarning("buff animator is null");
        }
    }

    public void AnimatedDeletion(GameObject GO)
    {
        GO.transform.DOKill();

        GO.transform
          .DOScale(Vector3.zero, 0.5f)
          .SetUpdate(true)
          .OnComplete(() => Destroy(GO));
    }

    public void RefreshChoicesButton()
    {
        CinemachineScript.Instance.ShakeUnscaled(0.5f, 0.15f);
        Debug.Log($"CurrentRefreses{currentRefreshes} and MaxRefreshed:{maxRefreshes}");
        AudioManager.Instance.PlaySoundFX("rerollSound", transform.position, 1f, 0.75f, 1.25f);
        currentRefreshes++;
        refreshButtonTextValue.text = (maxRefreshes - currentRefreshes).ToString();
        UpdateRefreshTextColor();
        RefreshChoices(false);

        if (currentRefreshes >= maxRefreshes)
        {
            Debug.Log("Hiding refresh button");
            refreshButtonGO.GetComponent<BaseButtonScript>().IsClicked = true;
            refreshButtonGO.transform.DOKill();
            refreshButtonGO.transform
                .DOScale(Vector3.zero, 0.3f)
                .SetEase(Ease.InBack)
                .SetUpdate(true)
                .OnComplete(() =>
                {
                    refreshButtonGO.GetComponent<BaseButtonScript>().IsClicked = false;
                    refreshButtonGO.SetActive(false);
                });
        }
    }

    private void UpdateRefreshTextColor()
    {
        if (maxRefreshes <= 0)
            return;

        float t = Mathf.Clamp01((float)currentRefreshes / maxRefreshes);
        t = Mathf.Pow(t, 0.7f);

        Color color;
        if (t < 0.5f)
        {
            color = Color.Lerp(Color.green, Color.yellow, t * 2f);
        }
        else
        {
            color = Color.Lerp(Color.yellow, Color.red, (t - 0.5f) * 2f);
        }

        refreshButtonTextValue.color = color;
    }


    private void SetNavigationGO(GameObject upgradeCoice)
    {
        StartCoroutine(SelectNextFrame(upgradeCoice));
    }

    private System.Collections.IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null; // wait for layout/tween

        if (go == null) yield break;

        var es = EventSystem.current;
        if (es == null) yield break;

        // Make absolutely sure it stays selected
        es.sendNavigationEvents = true;
        es.SetSelectedGameObject(null);
        yield return null;
        es.SetSelectedGameObject(go);

        if (go.TryGetComponent<Button>(out var btn))
            btn.Select();

        //Debug.Log("FORCED SELECTION ---> " + go.name);
    }

    private void ShowCatUi()
    {
        int catsCollected = GlobalVariables.Instance != null ? GlobalVariables.Instance.catsCollected : 0;
        int catsToShow = Mathf.Min(catsCollected, catsGOParent.childCount);
        for (int i = 0; i < catsGOParent.childCount; i++)
        {
            catsGOParent.GetChild(i).gameObject.SetActive(i < catsToShow);
        }
    }

    private List<UpgradeChoice> GetBonusBasedOnLuck(PlayerStatCategory upgradeCategory)
    {
        var result = new List<UpgradeChoice>();

        if (GlobalVariables.Instance == null || upgradeChoices == null || upgradeChoices.Count == 0)
            return result;

        int cats = Mathf.Clamp(GlobalVariables.Instance.catsCollected, 0, 15);
        if (cats <= 0)
            return result;

        // Get per-cat chances for extra bonuses
        var (chance1, chance2, chance3) = GetLuckChances(cats);

        // Roll how many extra bonuses we get (0–3)
        int bonusCount = 0;

        if (Random.value <= chance1)
            bonusCount++;

        if (bonusCount >= 1 && Random.value <= chance2)
            bonusCount++;

        if (bonusCount >= 2 && Random.value <= chance3)
            bonusCount++;

        if (bonusCount == 0)
            return result;

        // Filter valid choices from the same category that actually have bonuses
        var validChoices = upgradeChoices
            .Where(uc =>
                uc != null &&
                uc.UpgradeCategory == upgradeCategory)
            .OrderBy(_ => Random.value) // shuffle
            .ToList();

        if (validChoices.Count == 0)
            return result;

        // Take as many as we rolled (clamped to available)
        bonusCount = Mathf.Min(bonusCount, 3);

        for (int i = 0; i < bonusCount; i++)
        {
            result.Add(validChoices[i]);
            //Debug.Log($"Cat Luck Bonus: Added extra upgrade choice '{validChoices[i].name}' for category {upgradeCategory} (cats={cats}," +
            //    $" bonusCount={bonusCount}, UPGRADECODE:{validChoices[i].UpgradeCode})");
        }

        return result;
    }

    private (float chance1, float chance2, float chance3) GetLuckChances(int cats)
    {
        return cats switch
        {
            1 => (0.20f, 0.00f, 0.00f),
            2 => (0.30f, 0.10f, 0.00f),
            3 => (0.35f, 0.20f, 0.05f),
            4 => (0.40f, 0.25f, 0.08f),
            5 => (0.45f, 0.30f, 0.10f),
            6 => (0.50f, 0.35f, 0.12f),
            7 => (0.55f, 0.40f, 0.15f),
            8 => (0.60f, 0.45f, 0.18f),
            9 => (0.65f, 0.50f, 0.22f),
            10 => (0.70f, 0.55f, 0.26f),
            11 => (0.75f, 0.60f, 0.30f),
            12 => (0.80f, 0.65f, 0.35f),
            13 => (0.85f, 0.70f, 0.40f),
            14 => (0.90f, 0.75f, 0.45f),
            15 => (0.95f, 0.80f, 0.50f),
            _ => (0f, 0f, 0f),
        };
    }
}
