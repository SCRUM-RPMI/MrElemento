using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    
    private Rigidbody2D _rb; //RigidBody
    public GameObject habilidades; //Inventario de habilidades
    
    //SALUD
    private const int MaxHealth = 1000;
    public int currentHealth;

    public Image healthBarFill;
    public Text healthText;
    
    
    //HABILIDADES
    private Ability[] _abilities;
    
    private static RawImage _jumpPanel;

    private static bool _canDoubleJump;
    private static RawImage _doubleJumpPanel;
    private static bool _canWallJump;
    private static RawImage _wallJumpPanel;
    private static bool _canDash;
    private static RawImage _dashPanel;
    private static bool _canEmpoweredAttack;
    private static RawImage _empAttackPanel;
    
    public Ability jumpAb = new Ability(_jumpPanel);
    public Ability doubleJumpAb = new Ability(_doubleJumpPanel, _canDoubleJump);
    public Ability wallJumpAb = new Ability(_wallJumpPanel, _canWallJump);
    public Ability dashAb = new Ability(_dashPanel, _canDash);
    public Ability empAttackAb = new Ability(_empAttackPanel, _canEmpoweredAttack);
    
    private Color _colorActivo = Color.white;
    private Color _colorInactivo = Color.gray;
    
    //VELOCIDADES
    private float _speed = 8f;
    private float _wallSlidingSpeed = 2f;
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
        _rb = GetComponent<Rigidbody2D>();
        // Inicializa la salud actual al valor máximo
        currentHealth = MaxHealth;

        // Actualiza la barra de vida y el texto
        UpdateHealthBar();
        
        //Oculta el inventario
        habilidadesVis = false;
        
        //Inicializa a false las habilidades desbloqueables
        doubleJumpAb.unlocked = false;
        wallJumpAb.unlocked = false;
        dashAb.unlocked = false;
        empAttackAb.unlocked = false;
        
        //Crea un array con todas las habilidades
        _abilities = new Ability[] { jumpAb, doubleJumpAb, wallJumpAb, dashAb, empAttackAb };
    }

    void Update()
    {
        UpdateHealthBar();
        MostrarPanelHabilidades();
        habilidades.SetActive(habilidadesVis);
        //Añadir código aquí
    }

    void FixedUpdate()
    {
        //Añadir codigo aquí
    }
    
    // ACTUALIZAR LA BARRA DE SALUD
    void UpdateHealthBar()
    {
        currentHealth = (currentHealth > MaxHealth) ? MaxHealth : currentHealth;
        currentHealth = (currentHealth < 0) ? 0 : currentHealth;
        
        healthText.text = currentHealth + " / " + MaxHealth;

        // Calcula el porcentaje de vida y actualiza la imagen de la barra de vida
        float fillAmount = (float)currentHealth / MaxHealth;
        healthBarFill.color = (currentHealth >= MaxHealth / 2) ? 
            Color.green : (currentHealth > MaxHealth / 4) ? 
                Color.yellow : Color.red;
        healthBarFill.fillAmount = fillAmount;
    }
    
    // MOSTRAR PANEL DE HABILIDADES
    // ReSharper disable Unity.PerformanceAnalysis
    private void MostrarPanelHabilidades() //Tecla h
    {
        if (Input.GetKeyDown("h"))
        {
            habilidadesVis = !habilidadesVis;
            
            //Actualiza cómo se ven las habilidades en el panel según si están desbloqueadas o no

            foreach (var ability in _abilities)
            {
                foreach (var rawImage in ability.panelView.GetComponentsInChildren<RawImage>())
                {
                    rawImage.color = ability.unlocked ? _colorActivo : _colorInactivo;
                }

                ability.panelView.GetComponentInChildren<Toggle>().isOn = ability.unlocked;
            }
            
        };
    }

    // HABILIDADES

    // --Movimientos
    
    
    // --Ataques
    
    
    
}