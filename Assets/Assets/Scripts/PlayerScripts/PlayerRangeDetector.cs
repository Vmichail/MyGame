using System.Collections.Generic;
using UnityEngine;

public class PlayerRangeDetector : MonoBehaviour
{
    private List<GameObject> enemiesInRange = new List<GameObject>();
    public GameObject ClosestEnemy { get; private set; }
    public GameObject SecondClosestEnemy { get; private set; }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void Update()
    {
        UpdateClosestEnemies();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    void UpdateClosestEnemies()
    {
        enemiesInRange.RemoveAll(enemy => enemy == null);

        float closestDist = float.MaxValue;
        float secondClosestDist = float.MaxValue;
        GameObject closest = null;
        GameObject secondClosest = null;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue;

            float dist = Vector2.Distance(transform.position, enemy.transform.position);

            if (dist < closestDist)
            {
                // Shift current closest to second
                secondClosestDist = closestDist;
                secondClosest = closest;

                closestDist = dist;
                closest = enemy;
            }
            else if (dist < secondClosestDist)
            {
                secondClosestDist = dist;
                secondClosest = enemy;
            }
        }

        ClosestEnemy = closest;
        SecondClosestEnemy = secondClosest;
    }

    private void OnDrawGizmos()
    {
        if (ClosestEnemy != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(transform.position, ClosestEnemy.transform.position);
        }
    }

}
