using System.Collections;
using UnityEngine;

public class ParticlesGeneric : MonoBehaviour
{
    private ParticleSystem ps;
    private Coroutine returnRoutine;

    private void Awake()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnEnable()
    {
        if (returnRoutine != null)
            StopCoroutine(returnRoutine);

        ps.Play();

        returnRoutine = StartCoroutine(ReturnWhenFinished());
    }

    private IEnumerator ReturnWhenFinished()
    {
        yield return new WaitUntil(() => !ps.IsAlive(true));

        PoolManager.Instance.Return(gameObject);
    }
}
