// But : Programmer la classe mère des menus.
// Auteur : Gabriel Duquette Godon
// Date : 16 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

public abstract class Menu : MonoBehaviour
{
   #region Attribut
   public Animator CanvasAnimation { get; private set; }

   public EventSystem GestionnaireEvenement { get; private set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On relie la variable "canvasAnimation" avec le canvas du menu.
      CanvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On initialise le gestionnaire d'évènement.
      GestionnaireEvenement = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion

   #region Méthode publique
   public virtual void typeMenu()
   {
      Debug.Log("Je suis le menu abstrait des menus");
   }
   #endregion
}
