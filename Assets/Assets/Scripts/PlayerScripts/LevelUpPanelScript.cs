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
    private Color redColor = new Color(1f, 0f, 0f, 0.3f);
    private string[] levelUpSounds = new[] { "levelup1", "levelup2", "levelup3" };
    private void Awake()
    {
        allSpells = upgradeChoices.Where(uc => uc.UpgradeCategory == GlobalVariables.UpgradeCategory.Spells).ToList();
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
            UpgradeChoice choiceCopy = randomChoices[i];
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
                setUpgradeScript.SetUpgradeChoice(randomChoices[i], upgradeCost, isLocked, unlockCost);
                //Button
                Button button = upgradeChoice.GetComponent<Button>();
                if (button != null)
                {
                    if (isLocked)
                    {
                        Debug.Log($"Remove locked run for unlockCost:{unlockCost}, {i}, {button}, {choiceCopy} ");
                        button.onClick.AddListener(() => RemoveLock(unlockCost, i, button, choiceCopy, setUpgradeScript));
                    }
                    else
                    {
                        button.onClick.AddListener(() => BuyOption(choiceCopy, button.gameObject, setUpgradeScript));
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
                setUpgradeScript.SetUpgradeChoice(randomChoices[i], healthCost, false, 0);
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

    public void BuyOption(UpgradeChoice chosenUpgrade, GameObject buttonGO, SetUpgradeScript setUpgradeScript)
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
            GlobalVariables.Instance.UpgradeHero(chosenUpgrade.UpgradeCode);
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
        EnemyGenericFunctions.DamagePlayer(healthCost);
        TriggerBuff();
        setUpgradeScript.IsClicked = true;
        AudioManager.Instance.PlaySoundFX("cardChooseSound", transform.position, 1f, 0.75f, 1.25f);
        AnimatedDeletion(buttonGO);
        GlobalVariables.Instance.UpgradeHero(chosenUpgrade.UpgradeCode);
        Continue();
    }

    private void RemoveLock(int unlockCost, int index, Button button, UpgradeChoice upgradeChoice, SetUpgradeScript setUpgradeScript)
    {
        if (GlobalVariables.Instance.coinsCollected >= unlockCost)
        {
            Debug.Log($"Remove Locked Executed!!with index:{index}:buttonGameObjename:{button.gameObject.name}");
            upgradeScripts[index].RemoveLock(button, index, unlockCost);
            button.onClick.AddListener(() => BuyOption(upgradeChoice, button.gameObject, setUpgradeScript));
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

        Debug.Log("FORCED SELECTION ---> " + go.name);
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
}
