using UnityEngine;

public class RotatingLight : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // degrees per second

    private void OnEnable()
    {
        if (GlobalVariables.Instance.mainMenuScene)
        {
            gameObject.SetActive(false); // Stop rotation in the main menu
        }
        if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.LinaSprite.ToString()))
        {
            transform.localPosition = new Vector2(-0.22f, -0.29f);
        }
        else if (GlobalVariables.Instance.selectedCharacter.Equals(CharacterSprite.MiranaSprite.ToString()))
        {
            transform.localPosition = new Vector2(0.03f, -0.24f);
        }
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}