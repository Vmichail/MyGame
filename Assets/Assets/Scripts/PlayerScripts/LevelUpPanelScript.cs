using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private int healthCost = 10;
    private List<UpgradeChoice> randomChoices = new();
    private List<UpgradeChoice> allSpells = new();
    [SerializeField] private Animator buffAnimator;
    private List<SetUpgradeScript> upgradeScripts = new();
    [SerializeField] private GameObject refreshButtonGO;
    private int currentRefreshes = 0;
    public bool HealthCost = false;
    private Image levelUpPanelImage;
    private Color normalColor;
    private Color redColor = new(1f, 0f, 0f, 0.3f);
    private string[] levelUpSounds = new[] { "levelup1", "levelup2", "levelup3" };
    private void Awake()
    {
        allSpells = upgradeChoices.Where(uc => uc.UpgradeCategory == HeroUpgrades.UpgradeCategory.Spells).ToList();
        levelUpPanelImage = wholeLevelUpPanel.GetComponent<Image>();
        normalColor = levelUpPanelImage.color;
    }

    private void Update()
    {

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
        foreach (int i in Enumerable.Range(0, numberOfOptions))
        {
            UpgradeChoice randomFirstUpgrade = randomChoices[i];
            bool isLocked = false;
            int unlockCost = 0;
            GameObject upgradeChoice = Instantiate(upgradeOptionPrefab, upgradeChoicesParentTransform);
            upgradeChoice.name = "UpgradeOption" + i;
            if (upgradeChoice.TryGetComponent(out SetUpgradeScript setUpgradeScript))
            {
                upgradeScripts.Add(setUpgradeScript);
                if (i == 3 && (GlobalVariables.Instance == null || !GlobalVariables.Instance.isUpgradeOption4Unlocked))
                {
                    isLocked = true;
                    unlockCost = GlobalVariables.Instance ? GlobalVariables.Instance.upgradeOption4UnlockPrice : 25;
                }
                if (i == 4 && (GlobalVariables.Instance == null || !GlobalVariables.Instance.isUpgradeOption5Unlocked))
                {
                    isLocked = true;
                    unlockCost = GlobalVariables.Instance ? GlobalVariables.Instance.upgradeOption5UnlockPrice : 100;
                }
                List<UpgradeChoice> bonusUpgrades = GetBonusBasedOnLuck(randomFirstUpgrade.UpgradeCategory);
                setUpgradeScript.SetUpgradeChoice(randomFirstUpgrade, upgradeCost, isLocked, unlockCost, bonusUpgrades);
                //Button
                Button button = upgradeChoice.GetComponent<Button>();
                if (button != null)
                {
                    if (isLocked)
                    {
                        //Debug.Log($"Remove locked run for unlockCost:{unlockCost}, {i}, {button}, {randomFirstUpgrade} ");
                        button.onClick.AddListener(() => RemoveLock(unlockCost, i, button, randomFirstUpgrade, setUpgradeScript, bonusUpgrades));
                    }
                    else
                    {
                        button.onClick.AddListener(() => BuyOption(randomFirstUpgrade, button.gameObject, setUpgradeScript, bonusUpgrades));
                    }
                }
                if (i == 0)
                {
                    SetNavigationGO(upgradeChoice);
                }
            }

        }
    }

    public void ShowChoicesWithHealth()
    {
        levelUpPanelImage.color = redColor;
        upgradeScripts.Clear();
        foreach (Transform child in upgradeChoicesParentTransform)
        {
            Destroy(child.gameObject);
        }

        randomChoices = GetOnlySpells(3);
        foreach (int i in Enumerable.Range(0, 3))
        {
            UpgradeChoice choiceCopy = randomChoices[i];
            GameObject upgradeChoice = Instantiate(upgradeOptionPrefab, upgradeChoicesParentTransform);
            upgradeChoice.name = "UpgradeOption" + i;
            if (upgradeChoice.TryGetComponent(out SetUpgradeScript setUpgradeScript))
            {
                upgradeScripts.Add(setUpgradeScript);
                setUpgradeScript.HealthCost = true;
                setUpgradeScript.SetUpgradeChoice(randomChoices[i], healthCost, false, 0, new List<UpgradeChoice>());
                //Button
                if (upgradeChoice.TryGetComponent<Button>(out var button))
                {
                    button.onClick.AddListener(() => BuyOptionWithHealth(choiceCopy, button.gameObject, setUpgradeScript));
                }
            }
            if (i == 0)
            {
                SetNavigationGO(upgradeChoice);
            }
        }
        refreshButtonGO.SetActive(false);
    }

    public List<UpgradeChoice> GetRandomChoices(int count)
    {
        return upgradeChoices
            .OrderBy(x => Random.value)  // Random.value gives a float between 0–1
            .Take(count)
            .ToList();
    }

    public List<UpgradeChoice> GetOnlySpells(int count)
    {
        return allSpells
            .OrderBy(x => Random.value)  // Random.value gives a float between 0–1
            .Take(count)
            .ToList();
    }

    private void OnEnable()
    {
        ShowCatUi();
        AudioManager.Instance.PlayRandomSoundFX(levelUpSounds, transform.position, 1f, 1f, 1f);
        // Animate panel
        wholeLevelUpPanel.transform.localScale = Vector3.zero;
        if (!wholeLevelUpPanel.TryGetComponent<CanvasGroup>(out var cg)) cg = wholeLevelUpPanel.AddComponent<CanvasGroup>();
        cg.alpha = 0;

        LeanTween.scale(wholeLevelUpPanel, Vector3.one, 0.5f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);

        LeanTween.value(wholeLevelUpPanel, 0f, 1f, 0.4f)
            .setIgnoreTimeScale(true)
            .setOnUpdate((float val) => cg.alpha = val);

        // Animate refresh button
        refreshButtonGO.transform.localScale = Vector3.zero;
        LeanTween.scale(refreshButtonGO, Vector3.one, 0.3f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);


        currentRefreshes = 0;
        GlobalVariables.Instance.PauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);
        refreshButtonGO.SetActive(true);
        if (HealthCost)
        {
            ShowChoicesWithHealth();
        }
        else
            RefreshChoices(true);
        /* GlobalVariables.PauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);*/
        //Refresh Button
        LeanTween.scale(refreshButtonGO, Vector3.one, 0.3f)
            .setEaseInBack()
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                gameObject.SetActive(true);
            });
    }

    public void BuyOption(UpgradeChoice chosenUpgrade, GameObject buttonGO, SetUpgradeScript setUpgradeScript, List<UpgradeChoice> bonusChoices)
    {
        if (GlobalVariables.Instance.coinsCollected >= upgradeCost)
        {
            if (buttonGO.TryGetComponent(out Button button))
            {
                button.interactable = false;
            }
            GlobalVariables.Instance.coinsCollected -= upgradeCost;
            TriggerBuff();
            randomChoices.Remove(chosenUpgrade);
            upgradeCost += 10;
            UpdatePrices();
            setUpgradeScript.IsClicked = true;
            AudioManager.Instance.PlaySoundFX("cardChooseSound", transform.position, 1f, 0.75f, 1.25f);
            AnimatedDeletion(buttonGO);
            HeroUpgrades.Instance.UpgradeHero(chosenUpgrade);
            foreach (UpgradeChoice bonus in bonusChoices)
            {
                HeroUpgrades.Instance.UpgradeHero(bonus);
            }
            Continue();
        }
        else
        {
            LeanTween.scale(buttonGO, Vector3.one * 1.05f, 0.5f)
            .setEaseInOutSine()
            .setLoopPingPong(2)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
            {
                buttonGO.transform.localScale = Vector3.one;
            });
        }
    }

    public void BuyOptionWithHealth(UpgradeChoice chosenUpgrade, GameObject buttonGO, SetUpgradeScript setUpgradeScript)
    {
        if (buttonGO.TryGetComponent(out Button button))
        {
            button.interactable = false;
        }
        healthCost = (int)Mathf.Min(healthCost, GlobalVariables.Instance.playerCurrentHealth - 1);
        healthCost = Mathf.Max(healthCost, 0);
        UpdatePrices(healthCost);
        EnemyGenericFunctionsForPlayer.Instance.DamagePlayer(healthCost);
        TriggerBuff();
        setUpgradeScript.IsClicked = true;
        AudioManager.Instance.PlaySoundFX("cardChooseSound", transform.position, 1f, 0.75f, 1.25f);
        AnimatedDeletion(buttonGO);
        HeroUpgrades.Instance.UpgradeHero(chosenUpgrade);
        Continue();
    }

    private void RemoveLock(int unlockCost, int index, Button button, UpgradeChoice upgradeChoice, SetUpgradeScript setUpgradeScript, List<UpgradeChoice> bonusUpgrades)
    {
        if (GlobalVariables.Instance.coinsCollected >= unlockCost)
        {
            Debug.Log($"Remove Locked Executed!!with index:{index}:buttonGameObjename:{button.gameObject.name}");
            upgradeScripts[index].RemoveLock(button, index, unlockCost);
            button.onClick.AddListener(() => BuyOption(upgradeChoice, button.gameObject, setUpgradeScript, bonusUpgrades));
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
        AudioManager.Instance.PlayRandomSoundFX(GlobalVariables.Instance.linaAnnouncementsClips, transform.position, 1f, 1f, 1.25f);
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
        LeanTween.cancel(GO);
        LeanTween.scale(GO, Vector3.zero, 0.5f)
            .setIgnoreTimeScale(true)
            .setOnComplete(() => Destroy(GO));
    }

    public void RefreshChoicesButton()
    {
        AudioManager.Instance.PlaySoundFX("rerollSound", transform.position, 1f, 0.75f, 1.25f);
        currentRefreshes++;
        RefreshChoices(false);
        if (currentRefreshes >= GlobalVariables.Instance.maxRefreshBuyOptions)
        {
            LeanTween.cancel(refreshButtonGO);
            LeanTween.scale(refreshButtonGO, Vector3.zero, 0.3f)
                .setEaseInBack()
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    refreshButtonGO.SetActive(false);
                });
        }
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

    private List<UpgradeChoice> GetBonusBasedOnLuck(HeroUpgrades.UpgradeCategory upgradeCategory)
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
