using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ataque : MonoBehaviour
{
    private ElementosHUD hud;
    void Start()
    {
        hud = GetComponent<ElementosHUD>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //--detección de colisiones con enemigos--
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(30); // Reducir la salud cuando el jugador es atacado por un enemigo
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
        Debug.Log("¡El jugador ha muerto!");
    }
}
