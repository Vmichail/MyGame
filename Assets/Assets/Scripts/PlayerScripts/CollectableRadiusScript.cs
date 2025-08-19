using UnityEngine;

public class CollectableRadiusScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Collectable") && other.TryGetComponent<ICollectable>(out var collectable))
        {
            collectable.Collect();
        }
    }
}
