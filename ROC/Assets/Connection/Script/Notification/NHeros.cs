// But : Programmer les notification du menu d'héros
// Auteur : Gabriel Duquette Godon
// Date : 15 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class NHeros : Notification
{
   #region Attribut
   [SerializeField] GameObject niveauField;
   #endregion

   #region Méthode publique
   public override void chargementinfoNotification(string option)
   {
      // On fait la base de la notification
      base.chargementinfoNotification(option);

      // On analyse l'option choisis par le programmeur.
      switch (option)
      {
         case "NiveauInvalide":
            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change le texte de la description.
            definition.text = "Niveau du joueur doit être entre 1 à 30";

            // On change la direction du bouton de quitter.
            quitter.onClick.AddListener(retourMenuHeros);
            break;

         case "pasInformation":
            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change le texte de la description.
            definition.text = "Veuillez entrer un nombre entre 1 à 30";

            // On change la direction du bouton de quitter.
            quitter.onClick.AddListener(retourMenuHeros);
            break;
      }

      // On actionne l'animation pour faire apparaitre la fenêtre de notification.
      canvas.SetTrigger("NotificationMenu");
   }
   #endregion

   #region Méthode privées
   private void retourMenuHeros()
   {
      gestionnaireEvenement.SetSelectedGameObject(niveauField);

      canvas.SetTrigger("RetourHero");
   }
   #endregion
}
