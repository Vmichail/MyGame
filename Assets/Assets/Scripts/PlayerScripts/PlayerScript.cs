using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerRangeDetector rangeDetector;
    [SerializeField] private GameObject firstSpell;
    [SerializeField] private GameObject secondSpell;
    [SerializeField] private GameObject thirdSpell;
    [SerializeField] private GameObject forthSpell;
    GameObject closestEnemy;
    GameObject secondClosestEnemy;
    GameObject thirdClosestEnemy;
    GameObject forthClosestEnemy;
    Transform spriteTransform;
    public float originalY;



    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
        originalY = spriteTransform.position.y;
    }

    void Update()
    {
        if (rangeDetector.ClosestEnemy != null)
        {
            closestEnemy = rangeDetector.ClosestEnemy;
            secondClosestEnemy = rangeDetector.SecondClosestEnemy;
            thirdClosestEnemy = rangeDetector.ThirdClosestEnemy;
            forthClosestEnemy = rangeDetector.FourthClosestEnemy;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Attack");

        }
        else
        {
            animator.SetTrigger("Idle");
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer
        if (stateInfo.IsName("Attack"))
            animator.speed = 1f * GlobalVariables.Instance.playerAttackSpeed;
        else
            animator.speed = 1f;

    }

    public void RotatePlayer()
    {
        if (closestEnemy == null)
            return;
        if (closestEnemy.transform.position.x < 0)
        {
            spriteTransform.SetPositionAndRotation(
           new Vector3(-0.37f, originalY, 0),
           Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spriteTransform.SetPositionAndRotation(
            new Vector3(0, originalY, 0),
            Quaternion.Euler(0, 0, 0));
        }

    }

    public void CastFirstSpell()
    {
        RotatePlayer();
        if (closestEnemy == null)
            return;


        if (GlobalVariables.Instance.SecondSpellEnabled)
            CastSecondSpell();
        if (GlobalVariables.Instance.ThirdSpellEnabled)
            CastThirdSpell();
        if (GlobalVariables.Instance.ForthSpellEnabled)
            CastForthSpell();
        CastSpell(closestEnemy.transform.position, firstSpell);

    }



    public void CastSecondSpell()
    {
        Vector3 enemyPosition;
        if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastSpell(enemyPosition, secondSpell);

    }

    public void CastThirdSpell()
    {
        Vector3 enemyPosition;
        if (thirdClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastSpell(enemyPosition, thirdSpell);

    }

    public void CastForthSpell()
    {
        Vector3 enemyPosition;
        if (forthClosestEnemy != null)
        {
            enemyPosition = forthClosestEnemy.transform.position;
        }
        else if (closestEnemy != null)
        {
            enemyPosition = closestEnemy.transform.position;
        }
        else if (secondClosestEnemy != null)
        {
            enemyPosition = secondClosestEnemy.transform.position;
        }
        else if (thirdClosestEnemy != null)
        {
            enemyPosition = thirdClosestEnemy.transform.position;
        }
        else
        {
            return;
        }

        CastSpell(enemyPosition, forthSpell);

    }


    private void CastSpell(Vector3 enemyPosition, GameObject spell)
    {
        GameObject newSpell = Instantiate(spell, transform.position, Quaternion.identity);
        Vector2 direction = (enemyPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newSpell.transform.rotation = Quaternion.Euler(0, 0, angle);
        PlayerSpellBaseScript playerSpellBaseScript = newSpell.GetComponent<PlayerSpellBaseScript>();
        if (playerSpellBaseScript)
            playerSpellBaseScript.SetVelocity(direction, false);
    }
}
