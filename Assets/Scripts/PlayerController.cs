using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [System.Serializable]
    public class Ability
    {
        public RawImage panelView;
        public bool unlocked;

        public Ability(RawImage panelView, bool unlocked)
        {
            this.panelView = panelView;
            this.unlocked = unlocked;
        }
        public Ability(RawImage panelView)
        {
            this.panelView = panelView;
            this.unlocked = true;
        }
    }
    
    private Rigidbody2D rb; //RigidBody
    public GameObject habilidades; //Inventario de habilidades
    
    //HABILIDADES
    private Ability[] abilities;
    
    private static RawImage jumpPanel;
    
    private static bool canDoubleJump;
    private static RawImage doubleJumpPanel;
    private static bool canWallJump;
    private static RawImage wallJumpPanel;
    private static bool canDash;
    private static RawImage dashPanel;
    private static bool canEmpoweredAttack;
    private static RawImage empAttackPanel;
    
    public Ability jumpAb = new Ability(jumpPanel);
    public Ability doubleJumpAb = new Ability(doubleJumpPanel, canDoubleJump);
    public Ability wallJumpAb = new Ability(wallJumpPanel, canWallJump);
    public Ability dashAb = new Ability(dashPanel, canDash);
    public Ability empAttackAb = new Ability(empAttackPanel, canEmpoweredAttack);
    
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
        
        //Oculta el inventario
        habilidadesVis = false;
        
        //Inicializa a false las habilidades desbloqueables
        doubleJumpAb.unlocked = false;
        wallJumpAb.unlocked = false;
        dashAb.unlocked = false;
        empAttackAb.unlocked = false;
        
        abilities = new Ability[] { jumpAb, doubleJumpAb, wallJumpAb, dashAb, empAttackAb };
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
            
            //Muestra habilidades bloqueadas o desbloqueadas

            foreach (var ability in abilities)
            {
                foreach (var rawImage in ability.panelView.GetComponentsInChildren<RawImage>())
                {
                    rawImage.color = ability.unlocked ? colorActivo : colorInactivo;
                }

                ability.panelView.GetComponentInChildren<Toggle>().isOn = ability.unlocked;
            }
            
        };
    }

    // HABILIDADES

    // --Movimientos
    
        
    
    // --Ataques
    
    
    
}