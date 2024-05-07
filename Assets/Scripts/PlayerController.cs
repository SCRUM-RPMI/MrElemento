using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb; //RigidBody
    public GameObject habilidades; //Inventario de habilidades
    
    //HABILIDADES DESBLOQUABLES
    public bool canDoubleJump;
    public bool canDash;
    public bool canEmpoweredAttack;
    
    //VELOCIDADES
    private float speed = 8f;
    private float wallSlidingSpeed = 2f;
    [SerializeField] private float maxVel;
    
    //FUERZAS
    [SerializeField] private float jumpingPower;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    
    //COOLDOWNS
    [SerializeField] private float dashCD = 1f; //1 segundo
    
    //VARIABLES DE CONTROL
    private bool isFacingRight = true;
    
    //--Inventario de habilidades--
    public bool habilidadesVis;

    private bool isWallSliding;
    
    //--Jumping--
    private bool jump = true;
    private bool isJumping;
    
    //--Wall Jumping--
    private bool isWallJumping;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;

    //--Layer check--
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        //Inicializa a false las habilidades desbloqueables
        canDoubleJump = false;
        canDash = false;
        canEmpoweredAttack = false;
        
        //Oculta el inventario
        habilidadesVis = false;

    }

    void Update()
    {
        //Añadir código aquí
    }

    void FixedUpdate()
    {
        //Añadir codigo aquí
    }

    // HABILIDADES

    // --Movimientos
    
        
    
    // --Ataques
    
    
    
}