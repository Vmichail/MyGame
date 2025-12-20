using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using System;

public class DoorScript : MonoBehaviour
{
    public static DoorScript Instance;

    public bool doorIsUp = true;
    private Animator anim;
    private Collider2D doorCollider;
    private SpriteRenderer sr;

    [SerializeField] private float toggleInterval = 30f;

    private CancellationTokenSource cts;

    private void Awake()
    {
        Instance = this;

        anim = GetComponent<Animator>();
        doorCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartAutoLoop();
    }

    private void StartAutoLoop()
    {
        // cancel previous loop if any
        cts?.Cancel();
        cts = new CancellationTokenSource();
        AutoToggleLoop(cts.Token).Forget();
    }

    private async UniTaskVoid AutoToggleLoop(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                // Fix: Use correct UniTask.Delay overload
                await UniTask.Delay((int)(toggleInterval * 1000f), cancellationToken: token);

                if (!token.IsCancellationRequested)
                    ToggleDoor();
            }
        }
        catch (OperationCanceledException)
        {
            // expected when resetting the timer
        }
    }

    public void ToggleDoor()
    {
        if (doorIsUp) DoorDown();
        else DoorUp();
    }

    public void DoorDown()
    {
        if (!doorIsUp)
            return;

        anim.Play("DoorDown", 0, 0f);
    }

    public void DoorUp()
    {
        if (doorIsUp)
            return;
        anim.Play("DoorUp", 0, 0f);
    }

    public void EnableCollider()
    {
        doorCollider.enabled = true;
        doorIsUp = true;
        sr.sortingLayerID = SortingLayer.NameToID("Effects");
        sr.sortingOrder = 3;
        StartAutoLoop(); // reset timer
    }
    public void DisableCollider()
    {
        doorCollider.enabled = false;
        doorIsUp = false;
        sr.sortingLayerID = SortingLayer.NameToID("Ground");
        sr.sortingOrder = 0;
        StartAutoLoop(); // reset timer
    }

    private void OnDestroy()
    {
        cts?.Cancel();
        cts?.Dispose();
    }
}