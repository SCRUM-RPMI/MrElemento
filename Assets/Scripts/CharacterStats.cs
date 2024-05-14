using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{

    public int health;
    public int maxHealth;
    [SerializeField] protected bool isDead;
    public int level;

    private void Start()
    {
        InitVariables();
    }


    public virtual void CheckStats()
    {
        if (health <= 0)
        {
            health = 0;
            Die();
        }

        if (health >= maxHealth)
        {
            health = maxHealth;
        }
    }

    public void Die()
    {
        isDead = true;
    }

    private void SetHealthTo(int healthSetTo)
    {
        health = healthSetTo;
        CheckStats();
    }
    
    public void SetLevelTo(int levelAmount)
    {
        level = levelAmount;
    }

    public void TakeDamage(int damage)
    {
        int healthAfterDamage = health - damage;
        SetHealthTo(healthAfterDamage);
    }

    public void Heal(int heal)
    {
        int healthAfterHeal = heal + heal;
        SetHealthTo(healthAfterHeal);
    }

    public void InitVariables()
    {
        maxHealth = 100;
        SetHealthTo(maxHealth);
        isDead = false;

        level = 1;
        SetLevelTo(level);
    }

}
