using UnityEngine;
using UnityEngine.UI;

public class GameMenuPanel : MonoBehaviour
{
    [SerializeField] GameObject pauseGameOverPanel;
    [SerializeField] GameObject musicPanel;
    [SerializeField] Image backgroundImage;
    private Vector2 centerPosition;
    private Vector2 leftPosition;
    private bool isCentered = false;

    private void Start()
    {
        centerPosition = transform.position;
        leftPosition = new Vector2(-1250f, centerPosition.y);
        transform.position = leftPosition;
    }


    public void Update()
    {
        if ((pauseGameOverPanel.activeSelf || musicPanel.activeSelf) && !isCentered)
        {
            // Show and slide in
            backgroundImage.enabled = true;
            LeanTween.move(gameObject, centerPosition, 0.25f)
                     .setEase(LeanTweenType.easeOutCubic)
                     .setIgnoreTimeScale(true);
            isCentered = true;
        }
        else if (!(pauseGameOverPanel.activeSelf || musicPanel.activeSelf) && isCentered)
        {
            // Slide out and hide after tween finishes
            LeanTween.move(gameObject, leftPosition, 0.25f)
                     .setEase(LeanTweenType.easeInCubic)
                     .setIgnoreTimeScale(true)
                     .setOnComplete(() => backgroundImage.enabled = false);
            isCentered = false;
        }
    }

}
