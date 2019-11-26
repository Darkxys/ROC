// But : Programmer le menu de confirmation de fermeture d'application.
// Auteur : Gabriel Duquette Godon
// Date : 15 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
#endregion

public class Quitter : Menu
{
   #region Attribut
   [SerializeField] GameObject utilisateurField;
   #endregion

   #region Méthode publique
   /// <summary>
   /// Fonction qui permet le retour au menu de connection en sélectionne le champ d'utilisateur.
   /// </summary>
   public void retourMenuConnection()
   {
      // On met la zone saisis d'utilisateur active.
      GestionnaireEvenement.SetSelectedGameObject(utilisateurField);

      // On actionnne l'animation de retour au menu de connexion.
      CanvasAnimation.SetTrigger("RetourLogin");
   }     
   
   /// <summary>
   /// Fonction qui permet de quitter l'application.
   /// </summary>
   public void fermetureApplication()
   {
      // On quitte l'application.
      Application.Quit();
   }
   #endregion
}
