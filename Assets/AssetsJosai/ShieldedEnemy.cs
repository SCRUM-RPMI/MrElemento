
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 3f; // Adjust speed as needed
    private bool movingRight = true;
    private Vector3 originalScale;
    private bool obstacleDetected = false; // Flag to track if obstacle detected

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(MoveEnemy());
    }

    IEnumerator MoveEnemy()
    {
        while (true)
        {
            // Move right for 5 seconds
            if (movingRight)
            {
                Flip(false); // Face right
                float timer = 0f;
                while (timer < 5f)
                {
                    if (!obstacleDetected) // Check if no obstacle detected
                        transform.Translate(Vector2.right * speed * Time.deltaTime);
                    timer += Time.deltaTime;
                    yield return null;
                }
                movingRight = false;
            }
            // Move left for 5 seconds
            else
            {
                Flip(true); // Face left
                float timer = 0f;
                while (timer < 5f)
                {
                    if (!obstacleDetected) // Check if no obstacle detected
                        transform.Translate(Vector2.left * speed * Time.deltaTime);
                    timer += Time.deltaTime;
                    yield return null;
                }
                movingRight = true;
            }
        }
    }

    void Flip(bool facingRight)
    {
        if (facingRight)
            transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
        else
            transform.localScale = originalScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // If enemy collides with an obstacle, change direction
        if (collision.CompareTag("Obstacle"))
        {
            obstacleDetected = true;
            movingRight = !movingRight;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // Reset obstacle detection flag when obstacle is no longer in contact
        if (collision.CompareTag("Obstacle"))
            obstacleDetected = false;
    }
}


