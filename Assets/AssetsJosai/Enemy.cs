using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Velocidad de patrullaje del enemigo
    public float detectionRange = 5f; // Rango de detecci�n del jugador
    public float attackRange = 1f; // Rango de ataque del enemigo
    public int damage = 10; // Da�o que el enemigo inflige al jugador
    public Transform[] patrolPoints; // Puntos de patrullaje

    private int currentPatrolIndex;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private bool isChasing;
    private bool isAttacking;
    [SerializeField] private AudioClip chaseAudio;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Aseguramos que el audio no est� en loop
        currentPatrolIndex = 0;
        isChasing = false;
        isAttacking = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            StartAttacking();
        }
        else if (distanceToPlayer <= detectionRange)
        {
            StartChasing();
        }
        else
        {
            StopChasing();
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

        if (!isChasing)
        {
            UpdateEnemyDirection(targetPoint.position);
        }
    }

    void StartChasing()
    {
        if (!isAttacking)
        {
            isChasing = true;
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            UpdateEnemyDirection(player.position);
            PlayChaseAudio();
        }
    }

    void StopChasing()
    {
        isChasing = false;
        if (audioSource.isPlaying && audioSource.clip == chaseAudio)
        {
            audioSource.Stop();
        }
    }

    void StartAttacking()
    {
        isChasing = false;
        isAttacking = true;
        animator.SetTrigger("Ataque");
        AttackPlayer();
    }

    void UpdateEnemyDirection(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
        {
            transform.localScale = new Vector3(-3f, 3f, 3f);
        }
        else
        {
            transform.localScale = new Vector3(3f, 3f, 3f);
        }
    }

    void PlayChaseAudio()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = chaseAudio;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }
    }

    void AttackPlayer()
    {
        // Aqu� puedes implementar la l�gica de ataque, por ejemplo, reduciendo la salud del jugador.
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
