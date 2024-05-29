using UnityEngine;
using System.Collections;

public class FlyingEnemy : MonoBehaviour
{
    [SerializeField] float speed = 5.0f; // Speed of the flying enemy
    [SerializeField] float waitTimeMin = 5.0f; // Minimum wait time
    [SerializeField] float waitTimeMax = 10.0f; // Maximum wait time
    [SerializeField] Transform player; // Reference to the player's transform
    [SerializeField] float triggerDistance = 10.0f; // Distance to trigger the enemy behavior

    private Vector3 initialPosition; // Initial position of the enemy
    private bool isMoving = false; // Is the enemy currently moving

    void Start()
    {
        initialPosition = transform.position;
        StartCoroutine(EnemyBehavior());
    }

    IEnumerator EnemyBehavior()
    {
        while (true)
        {
            // Check if player is within trigger distance
            if (Vector3.Distance(transform.position, player.position) <= triggerDistance)
            {
                // Stay still for a random time
                float waitTime = Random.Range(waitTimeMin, waitTimeMax);
                yield return new WaitForSeconds(waitTime);

                // Move towards the player
                yield return StartCoroutine(MoveToPosition(player.position));

                // Return to initial position
                yield return StartCoroutine(MoveToPosition(initialPosition));
            }
            else
            {
                // If the player is not within range, wait for a short duration before checking again
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        isMoving = false;
    }
}

