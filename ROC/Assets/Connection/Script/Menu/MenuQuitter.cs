// But : Programmer le menu de quitter.
// Auteur : Gabriel Duquette Godon
// Date : 4 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

public class MenuQuitter : MonoBehaviour
{
   #region Attribut
   Animator canvasAnimation;   
   [SerializeField] GameObject utilisateurField;
   EventSystem gestionnaire;
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // Initialisation des attributs.
      canvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion

   #region Méthode publique
   /// <summary>
   /// Permettre à l’utilisateur de fermer l’application par l’option.
   /// </summary>
   public void fermetureApplication()
   {
      Application.Quit();
   }

   public void retourMenuLogin()
   {
      // On met la zone saisis d'utilisateur active.
      gestionnaire.SetSelectedGameObject(utilisateurField);
      
      // On renvoie l'utilisateur dans le menu de connexion.
      canvasAnimation.SetTrigger("RetourLogin");
   }
   #endregion
}
