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

   protected GameObject UtilisateurLoginField { get; private set; }

   public EventSystem GestionnaireEvenement { get; private set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On relie la variable "canvasAnimation" avec le canvas du menu.
      CanvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On relie la variable "UtilisateurLoginField" avec la zone de saisis du nom d'utilisateur de la connexion.
      UtilisateurLoginField = GameObject.FindGameObjectWithTag("UtilisateurConnection");

      // On initialise le gestionnaire d'évènement.
      GestionnaireEvenement = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion

   #region Méthode publique
   public virtual void retourMenuLogin()
   {
      // Note personnel, il saisis bien la zone de saisis mais il affiche pas la barre clinotante.

      // On met la zone de saisis d'utilisateur active.
      GestionnaireEvenement.SetSelectedGameObject(UtilisateurLoginField);

      // On met la zone de saisis d'utilisateur active.
      CanvasAnimation.SetTrigger("RetourLogin");
   }
   
   public virtual void typeMenu()
   {
      Debug.Log("Je suis le menu abstrait des menus");
   }
   #endregion
}
