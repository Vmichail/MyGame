using UnityEngine;
using UnityEngine.EventSystems;

public class CoinsBaseScript : MonoBehaviour
{
    Animator animator;
    private bool isCollected;

    public virtual int CoinValue => GlobalVariables.Instance.yellowCoinValue;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        if (animator == null)
        {
            Debug.LogWarning("Animator not found in children of " + gameObject.name);
        }
    }

    void Update()
    {
        if (!isCollected)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z);
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Collider2D col = Physics2D.OverlapPoint(worldPos);

            if (col != null && col.gameObject == gameObject)
            {
                TriggerCoin();
            }
        }
    }

    void OnEnable()
    {
        isCollected = false;
        animator.Rebind();
        animator.ResetTrigger("Clicked");
    }

    public void TriggerCoin()
    {
        isCollected = true;
        animator.SetTrigger("Clicked");
    }

    public void TriggerCoinStart()
    {
        GlobalVariables.Instance.coinsCollected += CoinValue;
    }

    public void TriggerCoinEnd()
    {
        CoinPoolScript.Instance.ReleaseCoin(gameObject);
    }
}
