using UnityEngine;
using System.Collections;  // Necesario para IEnumerator
using System.Collections.Generic;

public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] bool m_noBlood = false;
    [SerializeField] GameObject m_slideDust;

    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private ElementosHUD hud;
    private Sensor_HeroKnight m_groundSensor;
    private Sensor_HeroKnight m_wallSensorR1;
    private Sensor_HeroKnight m_wallSensorR2;
    private Sensor_HeroKnight m_wallSensorL1;
    private Sensor_HeroKnight m_wallSensorL2;
    private bool m_isWallSliding = false;
    private bool m_grounded = false;
    private bool m_rolling = false;
    private bool m_canDoubleJump = false; // Track if double jump is available
    private int m_facingDirection = 1;
    private int m_currentAttack = 0;
    private float m_timeSinceAttack = 0.0f;
    private float m_delayToIdle = 0.0f;
    private float m_rollDuration = 8.0f / 14.0f;
    private float m_rollCurrentTime;
    private bool isDead = false; // Track if the character is dead
    private double previousHealth;

    [SerializeField] private AudioClip atackAudio;
    [SerializeField] private AudioClip rollAudio;
    [SerializeField] private AudioClip deathAudio;
    [SerializeField] private AudioClip hitAudio;
    [SerializeField] private AudioClip blockAudio;
    [SerializeField] private AudioClip jumpAudio;
    [SerializeField] private AudioClip walkAudio;

    private AudioSource walkAudioSource;
    private bool isWalking = false;
    private int attackDamage = 50; // Daño del ataque

    private BoxCollider2D attackCollider;

    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        hud = GetComponent<ElementosHUD>();
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorR2 = transform.Find("WallSensor_R2").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<Sensor_HeroKnight>();
        m_wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<Sensor_HeroKnight>();
        previousHealth = hud.currentHealth;

        walkAudioSource = gameObject.AddComponent<AudioSource>();
        walkAudioSource.clip = walkAudio;
        walkAudioSource.loop = true; 
        walkAudioSource.playOnAwake = false;

        // Añadir BoxCollider2D para el ataque
        attackCollider = gameObject.AddComponent<BoxCollider2D>();
        attackCollider.isTrigger = true;
        attackCollider.enabled = false; // Desactivar inicialmente
    }

    // Update is called once per frame
    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;

        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_canDoubleJump = true;
        }

        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        if (hud.currentHealth < previousHealth)
        {
            previousHealth = hud.currentHealth;
            m_animator.SetTrigger("Hurt");
            soundEfectsController.instance.playSoundFXClip(hitAudio, transform, 0.2f);
        }

        if (!isDead && (Input.GetKeyDown("e") && !m_rolling || hud.currentHealth <= 0))
        {
            isDead = true;
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            soundEfectsController.instance.playSoundFXClip(deathAudio, transform, 0.3f);
        }
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);
            soundEfectsController.instance.playSoundFXClip(atackAudio, transform, 0.5f);
            m_timeSinceAttack = 0.0f;

            StartCoroutine(ActivateAttackCollider());
        }
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            soundEfectsController.instance.playSoundFXClip(blockAudio, transform, 0.3f);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
        }
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            StartRoll();
        }
        else if (Input.GetKeyDown("space"))
        {
            if (m_grounded && !m_rolling)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
                soundEfectsController.instance.playSoundFXClip(jumpAudio, transform, 0.3f);
            }
            else if (m_canDoubleJump && !m_rolling)
            {
                m_canDoubleJump = false;
                m_animator.SetTrigger("Jump");
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                soundEfectsController.instance.playSoundFXClip(jumpAudio, transform, 0.3f);
            }
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);

            if (m_grounded && !isWalking)
            {
                walkAudioSource.Play();
                isWalking = true;
            }
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;

            if (m_delayToIdle < 0)
            {
                m_animator.SetInteger("AnimState", 0);
                if (isWalking)
                {
                    walkAudioSource.Stop();
                    isWalking = false;
                }
            }
        }

        // Stop walk audio if character is not grounded
        if (!m_grounded && isWalking)
        {
            walkAudioSource.Stop();
            isWalking = false;
        }
    }

    private IEnumerator ActivateAttackCollider()
    {
        attackCollider.enabled = true;
        yield return new WaitForSeconds(0.1f);
        attackCollider.enabled = false;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
            }
        }

        
    }

    private void StartRoll()
    {
        m_rolling = true;
        m_rollCurrentTime = 0.0f;
        m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        m_animator.SetTrigger("Roll");
        soundEfectsController.instance.playSoundFXClip(rollAudio, transform, 0.5f);
    }

    private void CreateDust()
    {
        Vector3 spawnPosition = m_facingDirection == 1 ? m_wallSensorR2.transform.position : m_wallSensorL2.transform.position;

        if (m_slideDust != null)
        {
            GameObject dust = Instantiate(m_slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            dust.transform.localScale = new Vector3(m_facingDirection, 1, 1);
        }
    }
}
