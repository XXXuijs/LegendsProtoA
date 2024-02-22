using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraChallengeEnemy : MonoBehaviour
{
    public Stats enemyStats;

    [Tooltip("The transform to which the enemy will pace back and forth.")]
    public Transform[] patrolPoints;

    private int currentPatrolPoint = 1; // Changed initialization to 1

    [System.Serializable]
    public struct Stats
    {
        [Header("Enemy Settings")]
        public float speed;
        public bool move;
    }

    void Update()
    {
        if (enemyStats.move)
        {
            Vector3 moveToPoint = patrolPoints[currentPatrolPoint].position;
            transform.position = Vector3.MoveTowards(transform.position, moveToPoint, enemyStats.speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, moveToPoint) < 0.01f)
            {
                currentPatrolPoint++;

                if (currentPatrolPoint >= patrolPoints.Length)
                {
                    currentPatrolPoint = 0;
                }
            }
        }
    }
}