using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rb;

    //Velocidad
    [SerializeField] private float m_vel = 4.0f;
    [SerializeField] private float m_jumpForce = 7.5f;
    [SerializeField] private float m_rollForce = 6.0f;
    
    //Cooldowns
    [SerializeField] private float dash_CD = 1f; //1 segundo
    [SerializeField] private float t_cooldown; //Esto no sé qué hace

    //Habilidades desbloqueables
    public bool can_double_jump, can_dash, can_empowered_attack;

    //Variables de control
    public bool notDoubleJumping;
    
    //Inventario de habilidades
    public bool habilidades; 
    public GameObject Habilidades;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        //Inicializa a false las habilidades desbloqueables
        can_double_jump = false;
        can_dash = false;
        can_empowered_attack;
        //Oculta el inventario
        habilidades = false;
        //Establecemos variables de control
        notDoubleJumping = true;
    }

    void Update()
    {
        IncrementarTimer();
        ComprobarRodar();
        ComprobarAterrizaje();
        ComprobarCaida();
        MoverPersonaje();
        ManejarAnimaciones();
    }

    void FixedUpdate()
    {
        if (m_rolling)
        {
            m_body2d.velocity = new Vector2(m_facingDirection * m_rollForce, m_body2d.velocity.y);
        }
    }
    
    

    // HABILIDADES

    // Movimientos
    void Idle()
    {
        m_delayToIdle -= Time.deltaTime;
        if (m_delayToIdle < 0)
            m_animator.SetInteger("AnimState", 0);
    }

    void Flip(float inputX)
    {
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
    }

    void Move()
    {
        float inputX = Input.GetAxis("Horizontal");

        CambiarDireccion(inputX);

        if (!m_rolling)
            m_body2d.velocity = new Vector2(inputX * m_speed, m_body2d.velocity.y);

        m_animator.SetFloat("AirSpeedY", m_body2d.velocity.y);

        if (Input.GetKeyDown("space"))
            Saltar();

        if (Mathf.Abs(inputX) > Mathf.Epsilon)
            Correr();
        else
            EstarQuieto();
    }

    void Jump()
    {
        if (m_grounded && !m_rolling) //Está en el suelo y no está rodando
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
            m_groundSensor.Disable(0.2f);
        }
        else if (can_double_jump && notDoubleJumping && !m_rolling) //No está en el suelo y puede doublejump
        {
            notDoubleJumping = false;
            m_animator.SetTrigger("Jump");
            m_body2d.velocity = new Vector2(m_body2d.velocity.x, m_jumpForce);
        }
    }
    void Run()
    {
        m_delayToIdle = 0.05f;
        m_animator.SetInteger("AnimState", 1);
    }

    private void Dash()
    {
        if (dash && dash_CD <= 0f)
        {
            Vector2 dir = new Vector2(Input.GetAxis("Horizontal"), 0);
            if (dir.x == 0)
            {
                if (GetComponent<SpriteRenderer>().flipX)
                    dir.x = -1;
                else
                    dir.x = 1;
            }

            if (Input.GetAxis("Fire1") > 0)
            {
                _rb.drag = 5f;
                _rb.AddForce(dir * 20, ForceMode2D.Impulse);
                dash_CD = t_cooldown;
            }
        }
    }

    // Ataques
    void Attack()
    {
        m_currentAttack++;

        if (m_currentAttack > 3)
            m_currentAttack = 1;

        if (m_timeSinceAttack > 1.0f)
            m_currentAttack = 1;

        m_animator.SetTrigger("Attack" + m_currentAttack);
        m_timeSinceAttack = 0.0f;
    }

    void EmpoweredAttack()
    {
        if (empowered_attack)
        {
            //Código aquí
        }
    }
    
    // Fin ataques

    void IncrementarTimer()
    {
        m_timeSinceAttack += Time.deltaTime;

        if (m_rolling)
            m_rollCurrentTime += Time.deltaTime;

        if (m_rollCurrentTime > m_rollDuration)
            m_rolling = false;
    }

    void ComprobarRodar()
    {
        if (!m_rolling && Input.GetKeyDown("left shift") && !m_isWallSliding)
        {
            StartRoll();
        }
    }

    void ComprobarAterrizaje()
    {
        if (!m_grounded && m_groundSensor.State())
        {
            m_grounded = true;
            m_animator.SetBool("Grounded", m_grounded);
            m_canDoubleJump = true;
        }
    }

    void ComprobarCaida()
    {
        if (m_grounded && !m_groundSensor.State())
        {
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
        }
    }
    
    void ManejarAnimaciones()
    {
        ComprobarDeslizarPared();

        if (Input.GetKeyDown("e") && !m_rolling)
        {
            m_animator.SetBool("noBlood", m_noBlood);
            m_animator.SetTrigger("Death");
        }
        else if (Input.GetKeyDown("q") && !m_rolling)
            m_animator.SetTrigger("Hurt");

        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_rolling)
            Atacar();

        if (Input.GetMouseButtonDown(1) && !m_rolling)
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
            m_animator.SetBool("IdleBlock", false);
    }

    void ComprobarDeslizarPared()
    {
        m_isWallSliding = (m_wallSensorR1.State() && m_wallSensorR2.State()) ||
                          (m_wallSensorL1.State() && m_wallSensorL2.State());
        m_animator.SetBool("WallSlide", m_isWallSliding);
    }
}