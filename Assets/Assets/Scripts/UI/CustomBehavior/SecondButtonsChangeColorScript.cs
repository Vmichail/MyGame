using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEditor;

public class SecondButtonsChangeColorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    public List<Image> targetImages = new();
    public List<Color> normalColors = new();

    private void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>(true); // true = include inactive objects
        foreach (var img in images)
        {
            targetImages.Add(img);
            normalColors.Add(img.color);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetColor(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(false);
    }

    private void SetColor(bool highlight)
    {
        for (int i = 0; i < targetImages.Count; i++)
        {
            if (targetImages[i].gameObject.CompareTag("iconImage"))
            {
                continue;
            }
            else if (i == 0)
            {
                targetImages[i].color = highlight ? normalColors[i] + new Color(0, 0.1f, 0, 0) : normalColors[i];
            }
            else
            {
                targetImages[i].color = highlight ? normalColors[i] + new Color(0, 0.15f, 0, 0) : normalColors[i];
            }
        }
    }
}
