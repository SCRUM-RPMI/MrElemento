using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ojo : MonoBehaviour
{
    public float velocidad = 5f; // Velocidad de movimiento del objeto
    public float tiempoMinParada = 1f; // Tiempo mínimo de parada
    public float tiempoMaxParada = 3f; // Tiempo máximo de parada
    private Vector3 direccion = Vector3.right; // Dirección inicial de movimiento
    private Rigidbody2D rb;
    public int damage = 10; // Daño que el enemigo inflige al jugador

    private Transform player;
    private Animator animator;
    private AudioSource audioSource;
    private bool isDead = false;

    [SerializeField] private AudioClip attackAudio;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    private GameObject boss;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoverAleatoriamente());
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject bossObject = GameObject.FindGameObjectWithTag("boss");
        SetBoss(bossObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false; // Aseguramos que el audio no esté en loop
        maxHealth = 100; // Asigna un valor mayor para maxHealth
        currentHealth = maxHealth;
    }

    void Update()
    {
        // Mover el objeto en la dirección actual
        rb.MovePosition(rb.position + new Vector2(direccion.x, direccion.y) * velocidad * Time.deltaTime);
    }

    IEnumerator MoverAleatoriamente()
    {
        while (true)
        {
            // Esperar un tiempo aleatorio
            float tiempoParada = Random.Range(tiempoMinParada, tiempoMaxParada);
            yield return new WaitForSeconds(tiempoParada);

            // Decidir una nueva dirección aleatoriamente
            if (Random.value > 0.5f)
            {
                direccion = Vector3.right;
            }
            else
            {
                direccion = Vector3.left;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        // Cambiar la dirección al chocar con una pared
        if (col.gameObject.CompareTag("Pared"))
        {
            direccion = -direccion;
        }

        if (col.gameObject.CompareTag("Player"))
        {
            TakeDamage(100);
        }
    }

    public void SetBoss(GameObject bossObject)
    {
        boss = bossObject;
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
        if (boss != null)
        {
            Destroy(boss);
        }
        Destroy(gameObject); // Destruir el objeto después de que el sonido de muerte haya terminado
    }
}
