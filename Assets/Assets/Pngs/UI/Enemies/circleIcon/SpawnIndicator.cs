using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class SpawnIndicator : MonoBehaviour
{
    public Vector3 targetScale = new Vector3(0.3f, 0.3f, 1f);
    public float scaleTime = 0.5f;
    public LeanTweenType leanTweenType = LeanTweenType.easeInOutQuad;
    public int loopCount = 3;
    public bool IsReadyToSpawn { get; set; }

    public void Start()
    {
        IsReadyToSpawn = false;
        LeanTween.scale(gameObject, targetScale, scaleTime)
           .setEaseInOutSine()
           .setLoopPingPong(loopCount)
           .setOnComplete(() =>
           {
               IsReadyToSpawn = true;
           });
    }
}
