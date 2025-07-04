using TMPro;
using UnityEngine;

public class UpgradeButtonBase : MonoBehaviour
{
    [SerializeField] private Animator buffAnimator;

    void Start()
    {
    }
    public void TriggerBuff()
    {
        if (buffAnimator != null)
        {
            buffAnimator.ResetTrigger("PlayBuff");
            buffAnimator.SetTrigger("PlayBuff");
        }
        else
        {
            Debug.LogWarning("buff animator is null");
        }
    }
}