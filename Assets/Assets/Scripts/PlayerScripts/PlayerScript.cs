using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerRangeDetector rangeDetector;
    [Header("--!!Attack Spells!!--")]
    [SerializeField] private GameObject firstSpell;
    [SerializeField] private GameObject secondSpell;
    [SerializeField] private GameObject thirdSpell;
    [SerializeField] private GameObject forthSpell;
    private GameObject closestEnemy;
    private GameObject secondClosestEnemy;
    private GameObject thirdClosestEnemy;
    private GameObject forthClosestEnemy;
    private Transform spriteTransform;
    [Header("--!!Move Functionallity!!--")]
    public float moveSpeed = 5f;
    public bool playerCanMove = true;
    private Vector2 movementInput;
    private Rigidbody2D rb;
    [Header("--!!ETC!!--")]
    [SerializeField] private GameObject target;

    public GameObject ClosestEnemy => closestEnemy;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target.transform.SetParent(null);

        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
    }

    void Update()
    {
        if (playerCanMove)
        {
            movementInput.x = Input.GetAxisRaw("Horizontal");
            movementInput.y = Input.GetAxisRaw("Vertical");
        }
        else
        {
            movementInput = Vector2.zero;
        }
        if (movementInput != Vector2.zero)
            animator.SetBool("Moving", true);
        else
            animator.SetBool("Moving", false);

        if (rangeDetector.ClosestEnemy != null)
        {
            closestEnemy = rangeDetector.ClosestEnemy;
            secondClosestEnemy = rangeDetector.SecondClosestEnemy;
            thirdClosestEnemy = rangeDetector.ThirdClosestEnemy;
            forthClosestEnemy = rangeDetector.FourthClosestEnemy;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Attack");

        }
        else if (movementInput == Vector2.zero)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle");
        }
        else if (Mathf.Abs(movementInput.x) != 0)
        {
            animator.ResetTrigger("Attack");
            animator.SetTrigger("Idle");
            RotatePlayer();
        }
        ActivateDeactivateTarget();
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // 0 = Base Layer
        if (stateInfo.IsName("Attack"))
            animator.speed = 1f * GlobalVariables.Instance.playerAttackSpeed;
        else
            animator.speed = 1f;

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movementInput.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void RotatePlayer()
    {
        if (movementInput.x < 0)
        {
            spriteTransform.SetPositionAndRotation(
                transform.position + new Vector3(-0.37f, 0, 0),
                Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spriteTransform.SetPositionAndRotation(
                transform.position,
                Quaternion.Euler(0, 0, 0));
        }
    }

    public void RotatePlayerToEnemy()
    {
        if (closestEnemy == null)
            return;

        if (closestEnemy.transform.position.x < transform.position.x)
        {
            spriteTransform.SetPositionAndRotation(
                transform.position + new Vector3(-0.37f, 0, 0),
                Quaternion.Euler(0, 180, 0));
        }
        else
        {
            spriteTransform.SetPositionAndRotation(
                transform.position,
                Quaternion.Euler(0, 0, 0));
        }

    }

    public void CastFirstSpell()
    {
        RotatePlayerToEnemy();
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

    private void ActivateDeactivateTarget()
    {
        if (rangeDetector.ClosestEnemy != null)
        {
            target.SetActive(true);
            target.transform.position = rangeDetector.ClosestEnemy.transform.position;
        }
        else
        {
            target.SetActive(false);
        }
    }
}
