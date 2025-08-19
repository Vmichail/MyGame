using UnityEngine;

public class MainShardScript : MonoBehaviour
{
    private void Update()
    {
        // Only check if parent is active
        if (gameObject.activeSelf)
        {
            bool allInactive = true;

            foreach (Transform child in transform)
            {
                if (child.gameObject.activeSelf)
                {
                    allInactive = false;
                    break;
                }
            }

            // If every child is inactive, disable parent
            if (allInactive)
            {
                ShardPoolScript.Instance.ReleaseShard(gameObject);
            }
        }
    }

    private void OnEnable()
    {
        // When parent is enabled, enable all children
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}