using UnityEngine;

public class YellowCoinAnimatorScript : MonoBehaviour
{
    private YellowCoinScript yellowCoinScript;

    private void Start()
    {
        yellowCoinScript = GetComponentInParent<YellowCoinScript>();

        if (yellowCoinScript == null)
        {
            Debug.LogWarning("YellowCoinScript not found in parent!");
        }
    }

    public void TriggerCoinStart()
    {
        yellowCoinScript.TriggerCoinStart();
    }

    public void TriggerCoinEnd()
    {
        yellowCoinScript.TriggerCoinEnd();
    }
}
