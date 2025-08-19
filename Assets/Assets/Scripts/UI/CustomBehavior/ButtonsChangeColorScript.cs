using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting.ReorderableList;

public class ButtonsChangeColorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private Image[] targetImages;
    private Color[] normalColor;
    private bool clicked;
    public Color highlightColor = new Color(0.1f, 0.1f, 0.1f, 1f);
    public bool isWhite;
    public bool clickColorNotStays;
    public Color highlightColorDarker = new Color(0.1f, 0.1f, 0.1f, 0f);
    public void ResetColors()
    {
        clicked = false;
        for (int i = 0; i < targetImages.Length; i++)
        {
            if (targetImages[i] != null)
                targetImages[i].color = normalColor[i];
        }
    }

    public void SetClicked(bool value)
    {
        clicked = value;
    }

    public void ForceHighlight()
    {
        for (int i = 0; i < targetImages.Length; i++)
        {
            if (targetImages[i] != null)
            {
                targetImages[i].color = normalColor[i] + highlightColor;
            }
        }
    }

    private void Awake()
    {
        int childCount = transform.childCount;
        targetImages = new Image[childCount];
        normalColor = new Color[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.GetComponent<Image>() != null)
            {
                targetImages[i] = child.GetComponent<Image>();
                normalColor[i] = targetImages[i].color;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickColorNotStays)
        {
            clicked = false;
        }
        else
        {
            clicked = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (clicked)
            return;
        SetColor(false);
    }

    private void SetColor(bool highligh)
    {
        int i = 0;
        foreach (var img in targetImages)
        {
            if (img != null)
                if (img.gameObject.CompareTag("iconImage") && !highligh)
                {
                    img.color = Color.white;
                }
                else if (highligh)
                {
                    if (isWhite)
                        img.color -= highlightColorDarker;
                    img.color += highlightColor;
                }
                else
                {
                    img.color = normalColor[i];
                }
            i++;
        }
    }
}
