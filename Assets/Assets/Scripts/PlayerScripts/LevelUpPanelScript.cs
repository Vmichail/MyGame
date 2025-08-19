using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class LevelUpPanelScript : MonoBehaviour
{
    [Header("Spell Data")]
    [Header("")]
    [SerializeField] private Transform upgradeChoicesParentTransform;
    [SerializeField] private GameObject wholeLevelUpPanel;
    [SerializeField] private List<UpgradeChoice> upgradeChoices = new();
    [SerializeField] int numberOfOptions = 5;
    [SerializeField] GameObject upgradeOptionPrefab;
    private int upgradeCost;
    private List<UpgradeChoice> randomChoices = new();
    [SerializeField] private Animator buffAnimator;
    [SerializeField] private Button continueButton;
    private List<SetUpgradeScript> upgradeScripts = new();
    [SerializeField] private GameObject refreshButtonGO;

    private void Start()
    {
    }

    private void Update()
    {

    }


    public void RefreshChoices(bool resetCost)
    {
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
            }

        }
    }

    public List<UpgradeChoice> GetRandomChoices(int count)
    {
        return upgradeChoices
            .OrderBy(x => Random.value)  // Random.value gives a float between 0–1
            .Take(count)
            .ToList();
    }

    private void OnEnable()
    {
        GlobalVariables.PauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);
        refreshButtonGO.SetActive(true);
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
        continueButton.gameObject.SetActive(false);
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
            UpdateContinueButton();
            setUpgradeScript.IsClicked = true;
            AnimatedDeletion(buttonGO);
            GlobalVariables.Instance.UpgradeHero(chosenUpgrade.UpgradeCode);
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

    private void RemoveLock(int unlockCost, int index, Button button, UpgradeChoice upgradeChoice, SetUpgradeScript setUpgradeScript)
    {
        if (GlobalVariables.Instance.coinsCollected >= unlockCost)
        {
            Debug.Log($"Remove Locked Executed!!with index:{index}:buttonGameObjename:{button.gameObject.name}");
            upgradeScripts[index].RemoveLock(button, index, unlockCost);
            button.onClick.AddListener(() => BuyOption(upgradeChoice, button.gameObject, setUpgradeScript));
        }
    }

    private void UpdateContinueButton()
    {
        continueButton.gameObject.SetActive(true);
        continueButton.interactable = false;
        if (upgradeCost == 0)
        {
            return;
        }
        else if (upgradeCost > 0 && !continueButton.interactable)
        {
            continueButton.interactable = true;
            LeanTween.scale(continueButton.gameObject, Vector3.one, 0.5f)
            .setEaseOutBack()
            .setIgnoreTimeScale(true);
        }
        else
        {
            LeanTween.scale(continueButton.gameObject, Vector3.one * 1.05f, 0.5f)
            .setEaseInOutSine()
            .setLoopPingPong(2)
            .setIgnoreTimeScale(true)
            .setOnComplete(() =>
             {
                 continueButton.transform.localScale = Vector3.one; // reset scale
             });

            LeanTween.rotateZ(continueButton.gameObject, 5f, 0.25f)
                .setEaseInOutSine()
                .setLoopPingPong(2)
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    continueButton.transform.rotation = Quaternion.identity; // reset rotation
                });
        }
    }

    /*    void DestroyGO(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        void InactiveGO(GameObject gameObject)
        {
            gameObject.SetActive(false);
        }*/

    private void UpdatePrices()
    {
        foreach (SetUpgradeScript upgradeScript in upgradeScripts)
        {
            upgradeScript.SetPrice(upgradeCost);
        }
    }



    public void Continue()
    {
        wholeLevelUpPanel.SetActive(false);
        GlobalVariables.UnPauseTime(GlobalVariables.PauseReasonEnum.LevelUpPanel);
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
        RefreshChoices(false);
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
