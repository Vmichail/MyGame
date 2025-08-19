using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BaseButtonScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public bool IsClicked { get; set; }

    [SerializeField] private bool destoyedInClick;

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!IsClicked)
        {
            LeanTween.cancel(gameObject);
            transform.localScale = Vector3.one;
        }
    }

    public virtual void HighlightButton()
    {
        LeanTween.scale(gameObject, Vector3.one * 1.1f, 0.25f)
            .setEaseInOutSine()
            .setLoopPingPong(1)
            .setIgnoreTimeScale(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (destoyedInClick)
        {
            if (gameObject.TryGetComponent(out Button button))
            {
                button.interactable = false;
            }
            IsClicked = true;
            LeanTween.cancel(gameObject);
            LeanTween.scale(gameObject, Vector3.zero, 0.3f)
                .setEaseInBack()
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    gameObject.SetActive(false);
                    button.interactable = true;
                    IsClicked = false;
                });
        }
    }

}
