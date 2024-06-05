using UnityEngine;
using System.Collections;

public class Ojo : MonoBehaviour
{
    public float velocidad = 5f;
    public float tiempoMinParada = 1f;
    public float tiempoMaxParada = 3f;
    private Vector3 direccion = Vector3.right;
    private Rigidbody2D rb;
    public int damage = 10;

    private Animator animator;
    private AudioSource audioSource;
    private bool isDead = false;

    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private float currentHealth;
    [SerializeField] private float maxHealth;

    private GameObject boss;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(MoverAleatoriamente());
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    void Update()
    {
        rb.MovePosition(rb.position + new Vector2(direccion.x, direccion.y) * velocidad * Time.deltaTime);
    }

    IEnumerator MoverAleatoriamente()
    {
        while (true)
        {
            float tiempoParada = Random.Range(tiempoMinParada, tiempoMaxParada);
            yield return new WaitForSeconds(tiempoParada);

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
        if (col.gameObject.CompareTag("Pared"))
        {
            direccion = -direccion;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Si el jugador choca con el ojo, aplica daño al ojo
            TakeDamage(damage);
        }
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
        if (boss != null)
        {
            Destroy(boss);
        }
        Destroy(gameObject);
    }
}
