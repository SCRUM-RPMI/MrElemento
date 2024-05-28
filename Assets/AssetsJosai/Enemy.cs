using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Velocidad de patrullaje del enemigo
    public float detectionRange = 5f; // Rango de detección del jugador
    public float attackRange = 1f; // Rango de ataque del enemigo
    public int damage = 10; // Daño que el enemigo inflige al jugador
    public Transform[] patrolPoints; // Puntos de patrullaje
    private int currentPatrolIndex;
    private Transform player;
    private Animator animator;
    public bool isChasing; // Variable para rastrear si el enemigo está persiguiendo al jugador
    private bool isAttacking;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentPatrolIndex = 0;
        isChasing = false; // Al inicio, el enemigo no está persiguiendo al jugador
        isAttacking = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            isChasing = false;
            isAttacking = true;
            animator.SetTrigger("Ataque");
            AttackPlayer();
        }
        else if (distanceToPlayer <= detectionRange)
        {

            ChasePlayer();
        }
        else
        {
            isChasing = false;
            Patrol();
        }
        animator.SetBool("isChasing", isChasing);
    }

    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, targetPoint.position) < 0.2f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
        }

        // Si no estamos persiguiendo al jugador, actualizamos la animación
        if (!isChasing)
        {
            animator.SetBool("isChasing", false);
            // Si estamos patrullando hacia la izquierda, invertimos la escala en X
            if (transform.position.x > targetPoint.position.x)
            {
                transform.localScale = new Vector3(-3f, 3f, 3f);
            }
            // Si estamos patrullando hacia la derecha, dejamos la escala en X sin modificar
            else
            {
                transform.localScale = new Vector3(3f, 3f, 3f);
            }
        }
    }

    void ChasePlayer()
    {
        if (!isAttacking)
        {
            // Movemos al enemigo hacia el jugador
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            // Cambiamos la variable isChasing a true ya que ahora estamos persiguiendo al jugador
            isChasing = true;
            // Actualizamos la animación
            animator.SetBool("isChasing", true);
            // Invertimos la escala en X según la dirección del jugador
            if (player.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-3f, 3f, 3f);
            }
            else
            {
                transform.localScale = new Vector3(3f, 3f, 3f);
            }
        }
    }

    void AttackPlayer()
    {

        // Aquí puedes implementar la lógica de ataque, por ejemplo, reduciendo la salud del jugador.
        //animator.SetTrigger("Attack");
        // Ejemplo: player.GetComponent<PlayerHealth>().TakeDamage(damage);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public void StopAttacking()
    {
        isAttacking = false;
    }
}
