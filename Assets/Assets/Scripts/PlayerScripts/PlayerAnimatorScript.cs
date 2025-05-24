using UnityEngine;

public class PlayerAnimatorScript : MonoBehaviour
{
    private PlayerScript playerScript;

    private void Start()
    {
        playerScript = GetComponentInParent<PlayerScript>();
    }

    public void CallCastFireball()
    {
        playerScript.CastFirstSpell();
    }


    public void CallRotatePlayer()
    {
        return;
        playerScript.RotatePlayer();
    }
}
