using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    private void SelfDestroy()
    {
        Destroy(gameObject);
    }

    private void DestroyParent()
    {
        Destroy(transform.parent.gameObject);
    }
}
