using System.Collections;
using UnityEngine;

public class boss : MonoBehaviour
{
    public float detectionRange = 5f; // Rango de detección del jugador
    public float attackRange = 1f; // Rango de ataque del enemigo
    public int damage = 10; // Daño que el enemigo inflige al jugador

    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private bool isDead = false;

    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Aseguramos que el audio no esté en loop
        maxHealth = 100; // Asigna un valor mayor para maxHealth
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Aquí puedes añadir la lógica de detección y ataque si es necesario
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TakeDamage(100);
        }
    }

    public void TakeDamage(int damage)
    {
        // Reducir la salud actual
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Asegura que la salud no sea menor que 0 ni mayor que la salud máxima

        // Verificar si el personaje ha muerto
        if (currentHealth <= 0)
        {
            if (!isDead)
            {
                isDead = true; // Asegurar que el estado de muerte solo se procese una vez
                soundEfectsController.instance.playSoundFXClip(deathAudio, transform, 0.8f);
                StartCoroutine(HandleDeath()); // Iniciar la corrutina para manejar la muerte
            }
        }
    }

    private IEnumerator HandleDeath()
    {
        // Esperar hasta que el sonido de muerte haya terminado de reproducirse
        yield return new WaitWhile(() => soundEfectsController.instance.isPlaying(deathAudio));
        Destroy(gameObject); // Destruir el objeto después de que el sonido de muerte haya terminado
    }
}
