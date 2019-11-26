// But : Programmer le menu de création de compte.
// Auteur : Gabriel Duquette Godon
// Date : 16 octobre 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CreationCompte : Menu
{
   #region Attribut
   [SerializeField] GameObject utilisateurField;
   [SerializeField] List<InputField> champsDeCreation;
   [SerializeField] NCreation notificationCreation;

   private BDCreation LaCreation { get; set; }
   #endregion

   #region Constantes
   private const int MIN_CARACTERE_NOM = 1;
   private const int MAX_CARACTERE_NOM = 15;

   private const int MIN_CARACTERE_MOTDEPASSE = 1;
   private const int MAX_CARACTERE_MOTDEPASSE = 15;
   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      LaCreation = GameObject.FindGameObjectWithTag("CreationSql").GetComponent<BDCreation>();
   }
   #endregion

   #region Méthode publique
   /// <summary>
   /// Créer un nouveau compte d'utilisateur dans la base de données.
   /// </summary>
   public void creationCompte()
   {
      // Déclaration de la variable validation.
      bool validation;

      // On vérifie les normes de création.
      validation = verificationConditionCreation();

      // Si la validation est valide, fait ceci.
      if(validation)
      {
         // On envoie la requête à la base de données et on stocke la réponse reçus.
         string reponse = LaCreation.CreationCompte();

         // On analyse la réponse.
         switch (reponse)
         {
            case "CreationCompteReussi":
               notificationCreation.changementInfo("CreationCompteReussi");
               break;

            case "MotPasseDifferentCreation":
               notificationCreation.changementInfo("DifferentMotDePasse");
               break;

            case "UtilisateurExiste":
               notificationCreation.changementInfo("UtilisateurExiste");
               break;
         }
      }
   }

   public void retourMenuLogin()
   {
      // Note personnel, il saisis bien la zone de saisis mais il affiche pas la barre clinotante.
      
      // On met la zone de saisis d'utilisateur active.
      GestionnaireEvenement.SetSelectedGameObject(utilisateurField);
      
      // On met la zone de saisis d'utilisateur active.
      CanvasAnimation.SetTrigger("RetourLogin");
   }

   public override void typeMenu()
   {
      Debug.Log("Je suis le menu de la création de compte");
   }
   #endregion

   #region Méthode privée
   private bool verificationConditionCreation()
   {
      // On regarde si le nom d'utilisateur est entre 1 à 15 caractère.
      if (champsDeCreation[0].text.Length >= MIN_CARACTERE_NOM && champsDeCreation[0].text.Length <= MAX_CARACTERE_NOM )
      {
         // On regarde si le mot de passe est entre 1 à 15 caractère.
         if(champsDeCreation[1].text.Length >= MIN_CARACTERE_MOTDEPASSE && champsDeCreation[1].text.Length <= MAX_CARACTERE_MOTDEPASSE)
         {
            // On regarde si les deux mot passe sont parail.
            if (champsDeCreation[1].text == champsDeCreation[2].text)
               return true;
            else
               notificationCreation.changementInfo("DifferentMotDePasse");
         }
         else
         {
            // On fait apparaitre la notification d'une erreur de grandeur.
            notificationCreation.changementInfo("GrandeurMotDePassePasValide");
         }
      }
      else
      {
         // On fait apparaitre la notification d'une erreur de grandeur.
         notificationCreation.changementInfo("GrandeurNomPasValide");
      }      
      
      return false;
   }
   #endregion
}
