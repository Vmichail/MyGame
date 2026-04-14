using UnityEngine;

public class CollectableRadiusScript : MonoBehaviour
{
    [SerializeField] SpriteRenderer collectCircleSr;
    [SerializeField] Vector2 linaOffSet = new Vector2(-0.22f, -0.29f);
    [SerializeField] Vector2 miranaOffSet = new Vector2(0.03f, -0.24f);
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Collectable") && other.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Collect();
        }
    }

    private void OnEnable()
    {
        if (gameObject.TryGetComponent(out SpriteRenderer sr) && GlobalVariables.Instance.mainMenuScene && collectCircleSr != null)
        {
            collectCircleSr.color = new Color(collectCircleSr.color.r, collectCircleSr.color.g, collectCircleSr.color.b, 0f);
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        }
        if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.LinaSprite.ToString()))
        {
            transform.localPosition = linaOffSet;
        }
        else if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.MiranaSprite.ToString()))
        {
            transform.localPosition = miranaOffSet;
        }
    }
}
