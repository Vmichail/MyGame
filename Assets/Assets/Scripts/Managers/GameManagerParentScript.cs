using UnityEngine;

public class GameManagerParentScript : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
