using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsBaseScript : MonoBehaviour, ICollectable
{
    Animator animator;
    private bool IsCollected { get; set; }

    public virtual float CoinValue => GlobalVariables.Instance.yellowCoinValue;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator not found in children of " + gameObject.name);
        }
    }

    void OnEnable()
    {
        // Ensure Animator is fully reset
        animator.Rebind();
        animator.Update(0f);
        animator.ResetTrigger("Collected");
        IsCollected = false;
    }

    public void Collect()
    {
        IsCollected = true;
        animator.SetTrigger("Collected");
    }

    public void TriggerCoinStart()
    {
        AudioManager.Instance.PlaySoundFX("coinSound", transform.position, 0.8f, 0.75f, 1.25f);
        GlobalVariables.Instance.coinsCollected += CoinValue;
    }

    public void TriggerCoinEnd()
    {
        CoinPoolScript.Instance.ReleaseCoin(gameObject);
    }
}
