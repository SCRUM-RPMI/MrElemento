using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    public class Habilidad
    {
        public RawImage imagen;
        public bool habilidadActivada;

        // Constructor para inicializar los atributos de la habilidad
        public Habilidad(RawImage imagen, bool habilidadActivada)
        {
            this.imagen = imagen;
            this.habilidadActivada = habilidadActivada;
        }
    }

    private Rigidbody2D rb; //RigidBody
    public GameObject habilidades; //Inventario de habilidades
    
    //HABILIDADES DESBLOQUABLES
    public bool canDoubleJump;
    public RawImage doubleJumpPanel;
    public bool canWallJump;
    public RawImage wallJumpPanel;
    public bool canDash;
    public RawImage dashPanel;
    public bool canEmpoweredAttack;
    public RawImage empAttackPanel;

    private RawImage[] habilidadesArray;
    
    
    public Color colorActivo = Color.white;
    public Color colorInactivo = Color.gray;
    
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
        MostrarPanelHabilidades();
        habilidades.SetActive(habilidadesVis);
        //Añadir código aquí
    }

    void FixedUpdate()
    {
        //Añadir codigo aquí
    }
    
    // MOSTRAR PANEL DE HABILIDADES
    private void MostrarPanelHabilidades() //Tecla h
    {
        if (Input.GetKeyDown("h"))
        {
            habilidadesVis = !habilidadesVis;
            
            //Muestra habilidades
            doubleJumpPanel.color = canDoubleJump ? colorActivo : colorInactivo;
            doubleJumpPanel.GetComponentInChildren<Toggle>().isOn = canDoubleJump;
            
            wallJumpPanel.color = canWallJump ? colorActivo : colorInactivo;
            wallJumpPanel.GetComponentInChildren<Toggle>().isOn = canWallJump;
            
            dashPanel.color = canDash ? colorActivo : colorInactivo;
            dashPanel.GetComponentInChildren<Toggle>().isOn = canDash;
            
            empAttackPanel.color = canEmpoweredAttack ? colorActivo : colorInactivo;
            empAttackPanel.GetComponentInChildren<Toggle>().isOn = canEmpoweredAttack;
        };
    }

    // HABILIDADES

    // --Movimientos
    
        
    
    // --Ataques
    
    
    
}