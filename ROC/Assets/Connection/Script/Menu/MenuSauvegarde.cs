// But : Programmer le menu de sauvegarde.
// Auteur : Gabriel Duquette Godon
// Date : 13 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

public class MenuSauvegarde : MonoBehaviour
{
   #region Attribut
   EventSystem gestionnaire;
   Animator canvasAnimation;
   [SerializeField] SaveHandler controllerSauvegarde;
   [SerializeField] GameObject creationBtn;
   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      // On initialise les attributs.
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique
   public void annuler()
   {
      // On sélectionne le bouton de création.
      gestionnaire.SetSelectedGameObject(creationBtn);

      // On renvoie l'utilisateur dans le menu d'héros.
      canvasAnimation.SetTrigger("RetourHero");
   }

   public void creation()
   {
      // On fait la création de la sauvegarde.
      controllerSauvegarde.Create();

      // On renvoie l'utilisateur dans le menu d'héros.
      canvasAnimation.SetTrigger("RetourHero");
   }
   
   public void quitter()
   {
      // On sélectionne le bouton de création.
      gestionnaire.SetSelectedGameObject(creationBtn);

      // On renvoie l'utilisateur dans le menu d'héros.
      canvasAnimation.SetTrigger("RetourHero");
   }
   #endregion

   #region Méthode privée
   private void initialisationAttribut()
   {
      // On relie l'animation du canvas avec "canvas".
      canvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On relie le gestionnaire avec le "EventSystem" des menus.
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion
}