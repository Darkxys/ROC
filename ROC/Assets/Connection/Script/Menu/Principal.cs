// But : Programmer les options avancées du menu.
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion

public class Principal : Menu
{
   #region Attribut
   private GameObject UtilisateurCreation { get; set; }
   private GameObject Quitter { get; set; }
   [SerializeField] NConnexion notificationConnexion;

   private BDConnexion LaConnexion { get; set; }
   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      // On initialise les attributs.
      LaConnexion = GameObject.FindGameObjectWithTag("ConnexionSql").GetComponent<BDConnexion>();

      UtilisateurCreation = GameObject.FindGameObjectWithTag("CreationUtilisateur");
      Quitter = GameObject.FindGameObjectWithTag("FermerConnexion");
   }
   #endregion

   #region Méthode publique

   public void Connexion()
   {
      // On envoie la connexion à la base de donnée.      
      string information = LaConnexion.Connexion();

      // On analyse l'information reçus et on envoie a bonne place.
      switch (information)
      {
         case "ConnexionReussi":
            SceneManager.LoadScene("Main");
            break;

         case "mauvaisMotDePasse":
            notificationConnexion.changementInfo("mauvaisMotDePasse");
            break;

         case "pasInformation":
            notificationConnexion.changementInfo("pasInformation");
            break;
      }
   }

   /// <summary>
   /// Permettre à l’utilisateur de fermer l’application par l’option.
   /// </summary>
   public void entrerMenuCreationCompte()
   {
      // On met la zone saisis d'utilisateur active.
      GestionnaireEvenement.SetSelectedGameObject(UtilisateurCreation);

      // On actionne l'animation de transition vers la fenêtre "Création Compte".
      CanvasAnimation.SetTrigger("CreationCompte");
   }

   public void entreConfirmationFermetureProgramme()
   {
      // On met le bouton quitter active.
      GestionnaireEvenement.SetSelectedGameObject(Quitter);

      // On actionne l'animation de transition vers la fenêtre "Quitter Application".
      CanvasAnimation.SetTrigger("MenuQuitter");
   }

   public override void typeMenu()
   {
      Debug.Log("Je suis le menu principal");
   }
   #endregion
}
