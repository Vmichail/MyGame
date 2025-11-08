using UnityEngine;

public class HealEffectScript : MonoBehaviour
{
    private Animator animator;

    private void OnEnable()
    {
        // Get the Animator from this GameObject
        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogWarning($"No Animator found on {gameObject.name}");
            return;
        }

        // Play the animation and start disable coroutine
        animator.Play("Effect");
    }

    private void HealEffectEnds()
    {
        gameObject.SetActive(false);
    }

}
