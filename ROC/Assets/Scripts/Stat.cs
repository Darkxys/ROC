using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Stat
{
   [SerializeField]
   private BarresHud barreVie;
   [SerializeField]
   private BarresHud barreXp;

   [Space]
   [Header("Vie")]
   [SerializeField]
   private float maxValVie;
   [SerializeField]
   private float currentValVie;

   [Space]
   [Header("XP")]
   [SerializeField]
   private float maxValXp;
   [SerializeField]
   private float currentValXp;

   //--------------------Vie---------------------------

   public float CurrentValVie {
      get
      {
         return currentValVie;
      }
      set 
      {
         this.currentValVie = Mathf.Clamp(value,0,MaxValVie);
         barreVie.Value = currentValVie;
      } 
   }



   public float MaxValVie {
      get
      {
        return maxValVie;
      }
      set
      {
         this.maxValVie = value;
         barreVie.MaxValue = maxValVie;
      }
   }
   //----------------------XP-------------------------
   public float CurrentValXp
   {
      get
      {
         return currentValXp;
      }
      set
      {
         this.currentValXp = Mathf.Clamp(GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>().Xp, 0, MaxValXp);
         barreXp.Value = currentValXp;
      }
   }
   public float MaxValXp
   {
      get
      {
         return maxValXp;
      }
      set
      {
         this.maxValXp = value;
         barreXp.MaxValue = maxValXp;
      }
   }

   //----------------------------------------------------------------

   public void InitialiseVie()
   {
      this.MaxValVie = maxValVie;
      this.CurrentValVie = currentValVie;
   }
   public void InitialiseXp()
   {
      this.MaxValXp = 100;
      this.CurrentValXp = currentValXp;
   }
}
