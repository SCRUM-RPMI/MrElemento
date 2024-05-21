using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
   private PlayerHUD hud;

   private void Start()
   {
      GetReferences();
      InitVariables();
   }

   private void GetReferences()
   {
      hud = GetComponent<PlayerHUD>();
   }

   public override void CheckStats()
   {
      base.CheckStats();
      hud.UpdateHealth(health, maxHealth,level);
   }
}
