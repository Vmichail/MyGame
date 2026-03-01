using UnityEngine;
using UnityEngine.EventSystems;

public class UICharactersScript : BaseButtonScript
{
    [SerializeField] private Animator UIAnimator;
    private static readonly int AttackHash = Animator.StringToHash("Attack");

    public override void HighlightButton()
    {
        base.HighlightButton();

        if (UIAnimator != null)
        {
            UIAnimator.SetBool(AttackHash, true);
        }
    }

    public override void ResetScale()
    {
        base.ResetScale();


        if (UIAnimator != null)
        {
            UIAnimator.SetBool(AttackHash, false);
        }
    }
}