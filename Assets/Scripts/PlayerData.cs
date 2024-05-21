using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class PlayerData
{
    public int level;
    public int health;
    public int maxHealth;
    public float[] position;


    public PlayerData( CharacterStats stats)
    {
        level = stats.level;
        health = stats.health;
        maxHealth = stats.maxHealth;
        
        position = new float[3];
        position[0] = stats.transform.position.x;
        position[1] = stats.transform.position.y;
        position[2] = stats.transform.position.z;
        
    }
}
