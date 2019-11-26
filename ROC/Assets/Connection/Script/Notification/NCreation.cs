// But : Programmer la fenêtre de notification de création de compte.
// Auteur : Gabriel Duquette Godon
// Date : 17 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class NCreation : Notification
{
   #region Variable
   [SerializeField] GameObject utilisateurCreationField;
   #endregion

   #region Méthode publique
   public override void changementInfo(string option)
   {
      // On fait le début de la méthode classique.
      base.changementInfo(option);

      // On analyse l'option choisis par le programmeur.
      switch(option)
      {
         case "CreationCompteReussi":
            // On change la couleur de la vignette à du vert.
            Vignette.GetComponent<Image>().color = VertClaire;

            // On change le texte de la description.
            Description.text = "Création de compte réussi";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourCreationCompte);
            break;

         case "GrandeurNomPasValide":
            // On change la couleur de la vignette en rouge.
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "Nom d'utilisatuer doit avoir 1 à 15 caractère.";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourCreationCompte);
            break;

         case "GrandeurMotDePassePasValide":
            // On change la couleur de la vignette en rougeé
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "Le mot de passe doit avoir 1 à 15 caractère.";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourCreationCompte);
            break;

         case "DifferentMotDePasse":
            // On change la couleur de la vignette en rougeé
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "Les mots de passe sont différents.";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourCreationCompte);
            break;

         case "UtilisateurExiste":
            // On change la couleur de la vignette en rougeé
            Vignette.GetComponent<Image>().color = RougeClaire;

            // On change le texte de la description.
            Description.text = "L'utilisateur existe déjà !!!!!!";

            // On change la direction du bouton de "quitter".
            QuitterBtn.onClick.AddListener(retourCreationCompte);
            break;
      }

      // On active la notification.
      CanvasAnimation.SetTrigger("NotificationMenu");
   }

   public override void typeNotification()
   {
      Debug.Log("Je suis la notification de création de compte.");
   }
   #endregion

   #region Méthode privées
   private void retourCreationCompte()
   {
      // On met la zone de saisis d'utilisateur active.
      GestionnnaireEvenement.SetSelectedGameObject(utilisateurCreationField);

      // On actionne l'animation de retour au menu de création de compte.
      CanvasAnimation.SetTrigger("RetourCreationCompte");
   }
   #endregion
}
