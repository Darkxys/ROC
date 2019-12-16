// But : Programmer le menu de confirmation de suppresion.
// Auteur : Gabriel Duquette Godon
// Date : 14 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

public class ConfirmationSuppresion : MonoBehaviour
{
   #region Attribut
   EventSystem gestionnaire;
   Animator canvasAnimation;
   [SerializeField] GameObject creationBtn;
   [SerializeField] SaveHandler controllerSauvegarde;
   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      // On initialise les attributs.
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique
   public void retour()
   {
      // On sélectionne le bouton de création.
      gestionnaire.SetSelectedGameObject(creationBtn);

      // On renvoie l'utilisateur dans le menu d'héros.
      canvasAnimation.SetTrigger("RetourHero");
   }

   public void suppresion()
   {
      // On supprime le héros.
      controllerSauvegarde.Delete();
   }
   #endregion

   #region Méthode privé
   private void initialisationAttribut()
   {
      // On relie l'animation du canvas avec "canvas".
      canvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On relie le gestionnaire avec le "EventSystem" des menus.
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion
}
