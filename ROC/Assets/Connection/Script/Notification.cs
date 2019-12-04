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
   public Text definition{ get; private set; }
   
   public Button quitter { get; private set; }
   
   public Animator canvas { get; private set; }

   public VignetteCouleur couleurVignette { get; private set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On initialise les attributs
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique

   /// <summary>
   /// Change les informations de la fenêtre de notification.
   /// </summary>
   /// <param name="option">L’information voulue.</param>
   public virtual void chargementinfoNotification(string option)
   {
      // On enlève toutes les anciennes fonctions du bouton « quitter ».
      quitter.onClick.RemoveAllListeners();


      // On analyse l’option choisie par le programmeur.
      switch (option)
      {
         case "CreationCompteReussi":
            // On change le texte de la notification.
            definition.text = "La création de comptes est une réussite. Vous pouvez maintenant vous connecter à notre jeu.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;

         case "GrandeurIncorrecte":
            // On change le texte de la notification.
            definition.text = "Veuillez entrer un nom d'utilisateur entre 1 à 15 caractères !!";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;

         case "pasInformation":
            // On change le texte de la notification.
            definition.text = "Veuillez entrer les informations de votre utilisateur !!";
            
            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;

         case "mauvaisMotDePasse":
            // On change le texte de la notification.
            definition.text = "Votre mot de passe n’est pas valide. Veuillez vérifier votre mot de passe et réessayer.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourConnection);
            break;

         case "MotPasseDifferentCreation":
            // On change le texte de la notification.
            definition.text = "Veuillez mettre le même mot de passe dans les deux champs de mot de passe";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;

         case "UtilisateurExiste":
            // On change le texte de la notification.
            definition.text = "Veuillez choisir un autre nom d’utilisateur, car il existe déjà.";

            // On change la direction du bouton de "quitter".
            quitter.onClick.AddListener(retourCreationCompte);
            break;
      }

      // On actionne l'animation pour faire apparaitre la fenêtre de notification.
      canvas.SetTrigger("NotificationMenu");
   }
   #endregion

   #region Méthode protégée

   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de connexion par le bouton « quitter ».
   /// </summary>
   protected virtual void retourConnection()
   {
      canvas.SetTrigger("RetourLogin");
      
   }

   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de création de comptes par le bouton « quitter ».
   /// </summary>
   protected virtual void retourCreationCompte()
   {
      canvas.SetTrigger("RetourCreationCompte");
   }
   #endregion

   #region Méthode privé
   private void initialisationAttribut()
   {
      // On relie la variable "description" au champs texte "description" de la notification.
      definition = GameObject.FindGameObjectWithTag("NDescription").GetComponent<Text>();

      // On relie la variable "quitterbtn" avec le boutton "quitter" de la notification.
      quitter = GameObject.FindGameObjectWithTag("NQuitter").GetComponent<Button>();

      // On relie l'animation du canvas avec "canvas".
      canvas = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On initialise la variable "couleurVignette".
      couleurVignette = new VignetteCouleur();
   }
   #endregion
}
