using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private Animator animator;

    [SerializeField] private PlayerRangeDetector rangeDetector;
    [SerializeField] private GameObject fireball;
    [SerializeField] private GameObject darkball;
    private bool lookingRight = true;
    private GlobalVariables.Direction direction = GlobalVariables.Direction.Left;
    GameObject closestEnemy;
    GameObject secondClosestEnemy;
    Transform spriteTransform;


    private void Start()
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        if (spriteRenderer != null)
            spriteTransform = spriteRenderer.transform;
        else
            Debug.LogWarning("No Sprite was found!");
    }

    void Update()
    {
        if (rangeDetector.ClosestEnemy != null)
        {
            closestEnemy = rangeDetector.ClosestEnemy;
            secondClosestEnemy = rangeDetector.SecondClosestEnemy;
            animator.ResetTrigger("Idle");
            animator.SetTrigger("Attack");

        }
        else
        {
            animator.SetTrigger("Idle");
        }

    }

    public void RotatePlayer()
    {
        /*Debug.Log("Trying to rotate player\nclosestEnemy.transform.position:" + closestEnemy.transform.position);*/
        if (closestEnemy.transform.position.x < 0)
            spriteTransform.rotation = new Quaternion(0, 180, 0, 0);
        else
        {
            spriteTransform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

    public void CastFireball()
    {
        RotatePlayer();
        if (closestEnemy == null)
            return;

        GameObject newFireball = Instantiate(fireball, transform.position, Quaternion.identity);
        if (GlobalVariables.DarkballEnabled)
            CastDarkball();

        Vector2 direction = (closestEnemy.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newFireball.transform.rotation = Quaternion.Euler(0, 0, angle);

        if (newFireball.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.linearVelocity = direction * GlobalVariables.level1FireballSpeed;
        }
    }

    public void CastDarkball()
    {
        if (secondClosestEnemy == null)
        {
            Debug.Log("Second Enemy is null?");
            return;
        }

        GameObject newDarkball = Instantiate(darkball, transform.position, Quaternion.identity);

        Vector2 direction = (secondClosestEnemy.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newDarkball.transform.rotation = Quaternion.Euler(0, 0, angle);
        PlayerSpellBaseScript playerSpellBaseScript = newDarkball.GetComponent<PlayerSpellBaseScript>();
        if (playerSpellBaseScript)
            playerSpellBaseScript.SetVelocity(direction);

    }

}
