// But : Programmer les notification la connexion de compte.
// Auteur : Gabriel Duquette Godon
// Date : 4 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class NConnexion : Notification
{
   #region Méthode publique
   public override void chargementinfoNotification(string option)
   {
      // On fait la base de la notification
      base.chargementinfoNotification(option);

      // On analyse l'option choisis par le programmeur.
      switch (option)
      {
         case "mauvaisMotDePasse":
            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change le texte de la description.
            definition.text = "Mauvais mot de passe saisis.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;

         case "pasInformation":
            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change le texte de la description.
            definition.text = "Pas d'information saisis.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;
      }

      // On actionne l'animation pour faire apparaitre la fenêtre de notification.
      canvas.SetTrigger("NotificationMenu");
   }
   #endregion
}
