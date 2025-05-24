using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeDetector : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();

    public GameObject ClosestEnemy { get; private set; }
    public GameObject SecondClosestEnemy { get; private set; }
    public GameObject ThirdClosestEnemy { get; private set; }
    public GameObject FourthClosestEnemy { get; private set; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void Update()
    {
        UpdateClosestEnemies();
    }

    void UpdateClosestEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null || !enemy.activeInHierarchy);

        float dist1 = float.MaxValue;
        float dist2 = float.MaxValue;
        float dist3 = float.MaxValue;
        float dist4 = float.MaxValue;

        GameObject closest = null;
        GameObject second = null;
        GameObject third = null;
        GameObject fourth = null;

        foreach (GameObject enemy in enemiesInRange)
        {
            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist < dist1)
            {
                dist4 = dist3;
                fourth = third;

                dist3 = dist2;
                third = second;

                dist2 = dist1;
                second = closest;

                dist1 = dist;
                closest = enemy;
            }
            else if (dist < dist2)
            {
                dist4 = dist3;
                fourth = third;

                dist3 = dist2;
                third = second;

                dist2 = dist;
                second = enemy;
            }
            else if (dist < dist3)
            {
                dist4 = dist3;
                fourth = third;

                dist3 = dist;
                third = enemy;
            }
            else if (dist < dist4)
            {
                dist4 = dist;
                fourth = enemy;
            }
        }
        ClosestEnemy = closest;
        SecondClosestEnemy = second;
        ThirdClosestEnemy = third;
        FourthClosestEnemy = fourth;
    }

    private void OnDrawGizmos()
    {
        if (ClosestEnemy != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, ClosestEnemy.transform.position);
        }

        if (SecondClosestEnemy != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, SecondClosestEnemy.transform.position);
        }

        if (ThirdClosestEnemy != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position, ThirdClosestEnemy.transform.position);
        }

        if (FourthClosestEnemy != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, FourthClosestEnemy.transform.position);
        }
    }
}