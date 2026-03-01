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
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}