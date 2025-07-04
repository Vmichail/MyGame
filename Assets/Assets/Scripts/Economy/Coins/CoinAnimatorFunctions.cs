using UnityEngine;

public class CoinAnimatorFunctions : MonoBehaviour
{
    private CoinsBaseScript coinsBaseScript;
    void Start()
    {
        coinsBaseScript = GetComponentInParent<CoinsBaseScript>();

        if (coinsBaseScript == null)
        {
            Debug.LogWarning("CoinsBaseScript not found in parent!");
        }
    }


    void triggerCoinStart()
    {
        coinsBaseScript.TriggerCoinStart();
    }

    void triggerCoinEnds()
    {
        coinsBaseScript.TriggerCoinEnd();
    }
}
