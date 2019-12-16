// But : Programmer les notification de création de compte.
// Auteur : Gabriel Duquette Godon
// Date : 4 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion

public class NCreation : Notification
{
   #region Attribut
   [SerializeField] GameObject utilisateurCreationField;
   #endregion

   #region Méthode publique
   public override void chargementinfoNotification(string option)
   {
      // On fait la base de la notification
      base.chargementinfoNotification(option);

      // On analyse l’option choisie par le programmeur.
      switch(option)
      {
         case "CreationCompteReussi":
            // On change le texte de la notification.
            definition.text = "La création de comptes est une réussite. Vous pouvez maintenant vous connecter à notre jeu.";

            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.vertClaire();

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);

            // On active les deux boutons d'action.
            JouerBtn.gameObject.SetActive(true);
            RevenirBtn.gameObject.SetActive(true);

            // On change la direction du bouton de "revenir".
            RevenirBtn.onClick.AddListener(retourConnection);

            // On change la direction du bouton pour jouer.
            JouerBtn.onClick.AddListener(JouerJeu);
            break;

         case "GrandeurIncorrecte":
            // On change le texte de la notification.
            definition.text = "Veuillez entrer un nom d'utilisateur entre 1 à 15 caractères !!";

            // on change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;

         case "MotPasseDifferentCreation":
            // On change le texte de la notification.
            definition.text = "Veuillez mettre le même mot de passe dans les deux champs de mot de passe";

            // on change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;

         case "pasInformation":
            // On change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change le texte de la description.
            definition.text = "Pas d'information saisis.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;

         case "UtilisateurExiste":
            // On change le texte de la notification.
            definition.text = "Veuillez choisir un autre nom d’utilisateur, car il existe déjà.";

            // on change la couleur de la vignette.
            vignette.GetComponent<Image>().color = couleurVignette.rougeClaire();

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;
      }

      // On actionne l'animation pour faire apparaitre la fenêtre de notification.
      canvas.SetTrigger("NotificationMenu");
   }
   #endregion

   #region Méthode privée
   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de création de comptes par le bouton « quitter ».
   /// </summary>
   private void retourCreationCompte()
   {
      canvas.SetTrigger("RetourCreationCompte");
   }

   private void JouerJeu()
   {
      data.jouer(utilisateurCreationField.GetComponent<InputField>().text);
   }
   #endregion
}
