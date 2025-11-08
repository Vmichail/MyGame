using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static DifficultyManager;

public class DifficultyPanelScript : MonoBehaviour
{
    [Header("Other scripts")]
    [SerializeField] private MenuGenericFunctions menuGenericFunctions;

    [Header("Buttons (assign)")]
    [SerializeField] private Button easyButton;
    [SerializeField] private Button normalButton;
    [SerializeField] private Button hardButton;
    [SerializeField] private Button insaneButton;

    [Header("Row Images to Tint (assign)")]
    [SerializeField] private Image easyImage;
    [SerializeField] private Image normalImage;
    [SerializeField] private Image hardImage;
    [SerializeField] private Image insaneImage;

    [Header("Colors")]
    [SerializeField] private Color selectedColor = Color.yellow;
    [SerializeField] private float colorTweenTime = 0.25f; // optional smooth tint

    private Color easyOriginal;
    private Color normalOriginal;
    private Color hardOriginal;
    private Color insaneOriginal;

    private void Awake()
    {
        // store original colors
        if (easyImage) easyOriginal = easyImage.color;
        if (normalImage) normalOriginal = normalImage.color;
        if (hardImage) hardOriginal = hardImage.color;
        if (insaneImage) insaneOriginal = insaneImage.color;

        // hook up button events (ok if also wired in Inspector)
        if (easyButton) easyButton.onClick.AddListener(() => OnSelected(DifficultyLevel.Easy));
        if (normalButton) normalButton.onClick.AddListener(() => OnSelected(DifficultyLevel.Normal));
        if (hardButton) hardButton.onClick.AddListener(() => OnSelected(DifficultyLevel.Hard));
        if (insaneButton) insaneButton.onClick.AddListener(() => OnSelected(DifficultyLevel.Insane));

        // Make sure buttons participate in keyboard nav
        SetAutoNav(easyButton);
        SetAutoNav(normalButton);
        SetAutoNav(hardButton);
        SetAutoNav(insaneButton);
    }

    private void Start()
    {
    }

    public void OnSelected(DifficultyLevel level)
    {
        DifficultyManager.Instance.SetDifficulty(level);
        UpdateVisuals(level);
        menuGenericFunctions.HideAllMenus();
    }

    private void UpdateVisuals(DifficultyLevel selected)
    {
        // reset all row colors
        if (easyImage) TweenColor(easyImage, easyOriginal);
        if (normalImage) TweenColor(normalImage, normalOriginal);
        if (hardImage) TweenColor(hardImage, hardOriginal);
        if (insaneImage) TweenColor(insaneImage, insaneOriginal);

        // tint the selected row only
        switch (selected)
        {
            case DifficultyLevel.Easy:
                if (easyImage) TweenColor(easyImage, selectedColor);
                break;
            case DifficultyLevel.Normal:
                if (normalImage) TweenColor(normalImage, selectedColor);
                break;
            case DifficultyLevel.Hard:
                if (hardImage) TweenColor(hardImage, selectedColor);
                break;
            case DifficultyLevel.Insane:
                if (insaneImage) TweenColor(insaneImage, selectedColor);
                break;
        }
    }

    private void TweenColor(Image img, Color target)
    {
        if (!img) return;

        // No tween lib? Just snap:
        // img.color = target;

        // With LeanTween (smooth, unscaled UI-friendly):
        LeanTween.value(img.gameObject, img.color, target, colorTweenTime)
                 .setOnUpdate((Color c) => img.color = c)
                 .setIgnoreTimeScale(true);
    }
    //Event System selection handling
    private void OnEnable()
    {
        // 1) ensure EventSystem exists and sends nav events
        if (EventSystem.current != null)
            EventSystem.current.sendNavigationEvents = true;
        else
        {
            Debug.LogWarning("DifficultyPanelScript: No EventSystem found in scene for UI navigation!");
        }

        // 2) select Easy the frame after the panel becomes active (layout must be ready)
        StartCoroutine(SelectNextFrame(easyButton != null ? easyButton.gameObject : null));
    }

    private System.Collections.IEnumerator SelectNextFrame(GameObject go)
    {
        yield return null; // wait 1 frame so UI is active & laid out
        if (go != null && go.activeInHierarchy)
        {
            EventSystem.current?.SetSelectedGameObject(null);
            EventSystem.current?.SetSelectedGameObject(go);

            // extra safety
            if (go.TryGetComponent<Button>(out var b))
            {
                b.Select();
                Debug.Log("DifficultyPanelScript: Selected " + go.name);
            }
        }
    }
    private void SetAutoNav(Button b)
    {
        if (!b) return;
        var nav = b.navigation;
        nav.mode = Navigation.Mode.Explicit;
        b.navigation = nav;
    }
}