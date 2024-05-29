using UnityEngine;
using UnityEngine.UI;

public class ElementosHUD : MonoBehaviour
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
    }
    
    private Rigidbody2D _rb; // RigidBody
    private GameManager _gameManager;

    // GAME OVER
    public GameObject gameOver;
    public bool gameOverVis;
    
    // SALUD
    [Header("Salud")]
    public int currentHealth;
    public Image healthBarFill;
    public Text healthText;
    public int MaxHealth = 1000;
    
    // HABILIDADES
    [Header("Panel de habilidades")]
    public GameObject habilidades;
    public bool habilidadesVis;
    
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
    
    [Header("Habilidades")]
    public Ability jumpAb = new Ability(_jumpPanel, true);
    public Ability doubleJumpAb = new Ability(_doubleJumpPanel, _canDoubleJump);
    public Ability wallJumpAb = new Ability(_wallJumpPanel, _canWallJump);
    public Ability dashAb = new Ability(_dashPanel, _canDash);
    public Ability empAttackAb = new Ability(_empAttackPanel, _canEmpoweredAttack);
    
    private Color _colorActivo = Color.white;
    private Color _colorInactivo = Color.gray;
    void Start()
    {
        // Inicializa la salud actual al valor máximo
        currentHealth = MaxHealth;

        // Actualiza la barra de vida y el texto
        UpdateHealthBar();
        
        // Oculta el inventario y el mensaje de game over
        habilidadesVis = false;
        gameOverVis = false;
        
        // Inicializa a false las habilidades desbloqueables
        doubleJumpAb.unlocked = true;
        wallJumpAb.unlocked = false;
        dashAb.unlocked = true;
        empAttackAb.unlocked = true;
        
        // Crea un array con todas las habilidades
        _abilities = new Ability[] { jumpAb, doubleJumpAb, wallJumpAb, dashAb, empAttackAb };
    }
    
    void Update()
    {
        UpdateHealthBar();
        MostrarGameOver();
        MostrarPanelHabilidades();
        habilidades.SetActive(habilidadesVis);
    }
    
    // ACTUALIZAR LA BARRA DE SALUD
    public void UpdateHealthBar()
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

    private void MostrarGameOver()
    {
        gameOver.SetActive(gameOverVis);
    }
    
    private void MostrarPanelHabilidades() //Tecla h
    {
        if (Input.GetKeyDown("h"))
        {
            habilidadesVis = !habilidadesVis;
            Time.timeScale = habilidadesVis ? 0 : 1;
            
            // Muestra habilidades bloqueadas o desbloqueadas

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
}
