// But : Programmer les changements de la fenêtre de notification.
// Auteur : Gabriel Duquette Godon
// Date : 2 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class Notification : MonoBehaviour
{
   #region Attribut
   [SerializeField] Text notificationText;
   [SerializeField] Text titreTxt;
   [SerializeField] Button notificationQuitter;
   [SerializeField] Animator canvasAnimation;
   #endregion

   #region Méthode publique

   /// <summary>
   /// Change les informations de la fenêtre de notification.
   /// </summary>
   /// <param name="option">L’information voulue.</param>
   public void chargementinfoNotification(string option)
   {
      // On enlève toutes les anciennes fonctions du bouton « quitter ».
      notificationQuitter.onClick.RemoveAllListeners();

      // On analyse l’option choisie par le programmeur.
      switch (option)
      {
         case "CreationCompteReussi":
            // On change le texte du titre de la notificatoin.
            titreTxt.text = "Notification : La création de comptes est réussite.";

            // On change le texte de la notification.
            notificationText.text = "La création de comptes est une réussite. Vous pouvez maintenant vous connecter à notre jeu.";

            // On change la direction du bouton de "quitter".
            notificationQuitter.onClick.AddListener(retourConnection);
            break;

         case "pasInformation":
            // On change le texte du titre de la notification.
            titreTxt.text = "Notification : Saisis d’information";
            
            // On change le texte de la notification.
            notificationText.text = "Veuillez entrer les informations de votre utilisateur !!";
            
            // On change la direction du bouton de "quitter".
            notificationQuitter.onClick.AddListener(retourConnection);
            break;

         case "mauvaisMotDePasse":
            // On change le text du titre de la notification.
            titreTxt.text = "Notification : mauvais mot de passe";
            
            // On change le texte de la notification.
            notificationText.text = "Votre mot de passe n’est pas valide. Veuillez vérifier votre mot de passe et réessayer.";

            // On change la direction du bouton de "quitter".
            notificationQuitter.onClick.AddListener(retourConnection);
            break;

         case "MotPasseDifferentCreation":
            // On change le text du titre de la notificatoin.
            titreTxt.text = "Notification : Deux mots de passe différents";

            // On change le texte de la notification.
            notificationText.text = "Veuillez mettre le même mot de passe dans les deux champs de mot de passe";

            // On change la direction du bouton de "quitter".
            notificationQuitter.onClick.AddListener(retourCreationCompte);
            break;

         case "UtilisateurExiste":
            // On change le text du titre de la notification.
            titreTxt.text = "Notification : Utilisateur existe déjà";

            // On change le texte de la notification.
            notificationText.text = "Veuillez choisir un autre nom d’utilisateur, car il existe déjà.";

            // On change la direction du bouton de "quitter".
            notificationQuitter.onClick.AddListener(retourCreationCompte);
            break;
      }

      // On actionne l'animation pour faire apparaitre la fenêtre de notification.
      canvasAnimation.SetTrigger("NotificationMenu");
   }
   #endregion

   #region Méthode privées

   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de connexion par le bouton « quitter ».
   /// </summary>
   private void retourConnection()
   {
      canvasAnimation.SetTrigger("RetourLogin");
   }

   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de création de comptes par le bouton « quitter ».
   /// </summary>
   private void retourCreationCompte()
   {
      canvasAnimation.SetTrigger("RetourCreationCompte");
   }
   #endregion
}
