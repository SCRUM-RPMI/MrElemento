using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; 
    public float detectionRange = 5f; 
    public float attackRange = 1f; 
    public int damage = 10; 
    public Transform[] patrolPoints; 

    private int currentPatrolIndex;
    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private bool isChasing;
    private bool isAttacking;
    private bool isDead = false;
    [SerializeField] private AudioClip chaseAudio;
    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; 
        currentPatrolIndex = 0;
        isChasing = false;
        isAttacking = false;
        maxHealth = 100;
        currentHealth = maxHealth;
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
            audioSource.volume = 0.2f;
            audioSource.Play();
        }
    }

    void AttackPlayer()
    {
        soundEfectsController.instance.playSoundFXClip(attackAudio, transform, 0.1f);
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Eliminar esta llamada para evitar que el enemigo reciba daño al chocar con el jugador
    }


    public void TakeDamage(int damage)
    {
        
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); 

       
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                isDead = true; 
                soundEfectsController.instance.playSoundFXClip(deathAudio, transform, 0.8f);
                StartCoroutine(HandleDeath()); 
            }
        }
    }

    private IEnumerator HandleDeath()
    {
        
        yield return new WaitWhile(() => soundEfectsController.instance.isPlaying(deathAudio));
        Destroy(gameObject); 
    }
}
