using UnityEngine;

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
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        m_timeSinceAttack += Time.deltaTime;

        // Increase timer that checks roll duration
        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        // Disable rolling if timer extends duration
        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;

        // Check if character just landed on the ground
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_canDoubleJump = true; // Reset double jump on landing
        }

        // Check if character just started falling
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }

        // -- Handle input and movement --
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
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

        // Move
        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        // Set AirSpeed in animator
        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        // -- Handle Animations --
        // Wall Slide
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) || (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);

        // Hurt
        if (hud.currentHealth < previousHealth)
        {
            previousHealth = hud.currentHealth; // Update previousHealth before playing the animation to avoid repeated triggers
            m_animator.SetTrigger("Hurt");
            soundEfectsController.instance.playSoundFXClip(hitAudio, transform, 0.8f);
        }

        // Death
        if (!isDead && (Input.GetKeyDown("e") && !m_rolling || hud.currentHealth <= 0))
        {
            isDead = true; // Set isDead to true to prevent repeated death actions
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
            soundEfectsController.instance.playSoundFXClip(deathAudio, transform, 0.3f);
        }
        // Attack
        else if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3)
                m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            m_animator.SetTrigger("Attack" + m_currentAttack);

            soundEfectsController.instance.playSoundFXClip(atackAudio, transform, 0.5f);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }
        // Block
        else if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            soundEfectsController.instance.playSoundFXClip(blockAudio, transform, 0.3f);
        }
        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
        // Roll
        else if (Input.GetKeyDown("left shift") && !m_rolling && !m_isWallSliding)
        {
            StartRoll();
        }
        // Jump
        else if (Input.GetKeyDown("space"))
        {
            if (m_grounded && !m_rolling)
            {
                m_animator.SetTrigger("Jump");
                m_grounded = false;
                m_animator.SetBool("Grounded", m_grounded);
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                m_groundSensor.Disable(0.2f);
                soundEfectsController.instance.playSoundFXClip(jumpAudio, transform, 0.8f);
            }
            else if (m_canDoubleJump && !m_rolling)
            {
                m_canDoubleJump = false;
                m_animator.SetTrigger("Jump");
                m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
                soundEfectsController.instance.playSoundFXClip(jumpAudio, transform, 0.8f);
            }
        }
        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        // Idle
        else
        {
            // Prevents flickering transitions to idle
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        // Update previousHealth at the end of Update to ensure it's always in sync with currentHealth
        previousHealth = hud.currentHealth;
    }

    void StartRoll()
    {
        m_rolling = true;
        m_animator.SetTrigger("Roll");
        m_rollCurrentTime = 0.0f; // Reset roll timer
        soundEfectsController.instance.playSoundFXClip(rollAudio, transform, 0.8f);
    }

    void FixedUpdate()
    {
        if (m_rolling)
        {
            // Keep rolling as long as the roll button is held down
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
    }
}
