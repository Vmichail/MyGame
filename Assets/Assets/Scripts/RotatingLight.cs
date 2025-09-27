using UnityEngine;

public class RotatingLight : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f; // degrees per second

    void Update()
    {
        // Rotate around Z axis (2D rotation)
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}