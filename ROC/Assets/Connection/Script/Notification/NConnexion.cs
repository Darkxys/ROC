// But : Programmer les notifications de la connexion.
// Auteur : Gabriel Duquette Godon
// Date : 16 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class NConnexion : Notification
{
   #region Variable
   [SerializeField] GameObject utilisateurField;
   #endregion

   #region Méthode publique
   
   public override void changementInfo(string option)
   {
      // On fait le début de la méthode classique.
      base.changementInfo(option);

      // On analyse l'option choisis par le programmeur.
      switch(option)
      {
         case "ConnexionReussi":
            // On change la couleur de la vignette de la notification pour du vert.
            Vignette.GetComponent<Image>().color = VertClaire;

            // On change le texte de la decription.
            Description.text = "Connexion réussite";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourConnexion);
            break;

         case "mauvaisMotDePasse":
            // On change la couleur de la vignette de la notification pour du rouge.
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "Mauvais mot de passe saisis.";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourConnexion);
            break;

         case "pasInformation":
            // On change la couleur de la vignette de la notification pour du rouge.
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "Pas d'information saisis.";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourConnexion);
            break;
      }

      // On active la notification.
      CanvasAnimation.SetTrigger("NotificationMenu");  
   }

   public override void typeNotification()
   {
      Debug.Log("Je suis la notification de la connexion");
   }
   #endregion

   #region Méthode privé
   private void retourConnexion()
   {
      // On met la zone de saisis d'utilisateur active.
      GestionnnaireEvenement.SetSelectedGameObject(utilisateurField);

      // On active l'animation de retour au menu de connexion.
      CanvasAnimation.SetTrigger("RetourLogin");
   }
   #endregion
}
