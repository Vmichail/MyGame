using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonsChangeColorScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    private Image[] targetImages;
    public Color normalColor;
    public Color highlightColor = new Color(1f, 1f, 1f, 0.1f); // Slightly lighter

    private void Awake()
    {
        int childCount = transform.childCount;
        targetImages = new Image[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            targetImages[i] = child.GetComponent<Image>();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SetColor(highlightColor, true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetColor(normalColor, false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetColor(normalColor, false);
    }

    private void SetColor(Color color, bool highligh)
    {
        foreach (var img in targetImages)
        {
            if (img != null)
                if (img.gameObject.CompareTag("iconImage") && !highligh)
                {
                    img.color = new Color(255, 255, 255, 255);
                }
                else
                {
                    img.color = color;
                }
        }
    }
}
