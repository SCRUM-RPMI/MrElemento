using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{
    private Rigidbody2D _rb;
    private ElementosHUD hud;
    void Start()
    {
        hud = GetComponent<ElementosHUD>();
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Spikes"))
        {
            TakeDamage(50); 
        }

        if (other.CompareTag("pinchoMuerte"))
        {
            TakeDamage(hud.currentHealth);
        }
    }
    
    //HAY QUE CAMBIAR ESTO:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(300); 
        }
    }

    public void TakeDamage(int damage)
    {
        hud.currentHealth -= damage;
        hud.currentHealth = Mathf.Clamp(hud.currentHealth, 0, hud.MaxHealth); // Asegura que la salud no sea menor que 0 ni mayor que la salud máxima
        hud.UpdateHealthBar(); // Actualiza la barra de salud
        if (hud.currentHealth <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        hud.gameOverVis = true;
        Time.timeScale = 0;
    }
}
