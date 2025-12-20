using Cysharp.Threading.Tasks;
using UnityEngine;

public class CollectablePetScript : CollectableBaseScript
{
    public virtual string[] CatMeowSounds
    {
        get => new string[] { "catMeow1", "catMeow2" };
    }

    private bool isActive;

    protected override void OnEnable()
    {
        base.OnEnable();
        isActive = true;
        MeowSoundLoop().Forget();
    }

    private void OnDisable()
    {
        isActive = false;
    }

    private async UniTaskVoid MeowSoundLoop()
    {
        while (isActive)
        {
            //AudioManager.Instance.PlayRandomSoundFX(CatMeowSounds, transform.position, 0.1f, 1f, 1f, applyDistance: true);

            await UniTask.Delay(5000, DelayType.DeltaTime, PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
        }
    }

    protected override void CollectEnds()
    {
        AudioManager.Instance.PlaySoundFX("catCollectSound", transform.position, 0.8f, 0.75f, 1.25f);
        GlobalVariables.Instance.catsCollected++;
        gameObject.SetActive(false);
        boxCollider.enabled = true;
        IsCollected = false;
    }
}