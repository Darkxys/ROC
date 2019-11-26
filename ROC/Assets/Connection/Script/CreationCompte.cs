// But : Programmer la création de compte pour le jeu ROC.
// Auteur : Gabriel Duquette Godon
// Date : 10 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MySql;   // On rajoute la bibliothèque MySql.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
#endregion

public class CreationCompte : MonoBehaviour
{
   #region Attribut

   #region Bouton
   public Button btn_Connection;
   public Button btn_Annuler;
   #endregion

   #region Zone texte
   public InputField Utilisateur;
   public InputField MotDePasse;
   public InputField Confirmation_MotDePasse;
   #endregion

   #region Regex
   private Regex alphanumerique = new Regex("^[a-zA-Z0-9]*$");
   #endregion

   #region Constances
   private const int MIN_CARACTERE = 1;
   private const int MAX_CARACTERE = 15;
   #endregion

   #region Système
   EventSystem systeme;
   DatabaseManager manager_bd;
   #endregion

   #endregion

   private void Start()
   {
      // On initialise la variable system.
      systeme = EventSystem.current;
      manager_bd = GetComponent<DatabaseManager>();
   }

   private void Update()
   {
      // On fait la tabulation si l'utilisateur clique sur tabulation.
      Tabulation();
   }

   #region Méthode publique

   #region Bouton
   public void Creation()
   {
      // On vérifie l'utilisateur respecte les normes.
      if (Verification_Utilisateur())
      {
         // On vérifie si le mot de passe respecte les normes.
         if (Verification_MotDePasse(false))
         {
            if (Verification_MotDePasse(true))
               manager_bd.Enregistrement();
            else
               SceneManager.LoadScene("Notification_MotDePasse", LoadSceneMode.Single);
         }
         else
            SceneManager.LoadScene("Notification_MotDePasse", LoadSceneMode.Single);
      }
      else
         SceneManager.LoadScene("Notification_Utilisateur", LoadSceneMode.Single);
   }
   
   public void Annuler()
   {
      SceneManager.LoadScene("Connection", LoadSceneMode.Single);
   }
   #endregion

   public bool Verification_Utilisateur()
   {
      // Minimum un caractère.
      // Maximum : 15 caractère.
      // Caractère alpha numérique
      // Case unsentitive.

      // On regarde si le mot de passe respect les normes du nombre de caractère.
      if (Utilisateur.text.Length < MIN_CARACTERE || Utilisateur.text.Length > MAX_CARACTERE)
         return false;   // S'il ne respecte pas les caractérisque, on retourne faux.

      // On regarde si le nom n'est alpha numérique.
      if (!alphanumerique.IsMatch(Utilisateur.text))
         return false; // S'il n'est pas alpha numérique, renvoie faux.

      // Si le nom respecte tous les cas, on retourne vrai.
      return true;
   }

   public bool Verification_MotDePasse(bool Confirmation)
   {
      // Déclaration des variables locales.
      InputField MotPasse_Verifier = (Confirmation ? Confirmation_MotDePasse : MotDePasse);
      
      // Minimum : un caractère.
      // Maximum : 15 caractère.
      // Caractère alpha numérique et caractères spéciaux.
      // Case ensitive.

      // On regarde si le mot de passe respect les normes du nombre de caractère.
      if (MotPasse_Verifier.text.Length < MIN_CARACTERE || MotPasse_Verifier.text.Length > MAX_CARACTERE)
         return false;   // S'il ne respecte pas les caractérisque, on retourne faux.


      if(Confirmation)
      {
         // Si les deux mots de passe n'est pas parail alors retourne faux.
         if (MotPasse_Verifier.text != MotDePasse.text)
            return false;
      }

      return true;
   }

   #endregion

   #region Méthode Privées
   private void Tabulation()
   {
      // Lien internet qui m'a aider configuerer la méthode : https://forum.unity.com/threads/tab-between-input-fields.263779/

      // Si l'utilisateur clique sur la tabulation, fait ceci.
      if (Input.GetKeyDown(KeyCode.Tab))
      {
         // On crée une variable qui va sélectionner la prochain objet.
         Selectable prochain = systeme.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

         // Si le prochain objet n'est pas null, fait ceci.
         if (prochain != null)
         {
            // On crée une variable bouton qui va trouver le prochain bouton.
            InputField bouton = prochain.GetComponent<InputField>();

            // Si le prochain bouton n'est pas null, fait ceci.
            if (bouton != null)
               bouton.OnPointerClick(new PointerEventData(systeme));

            systeme.SetSelectedGameObject(prochain.gameObject, new BaseEventData(systeme));
         }
      }
   }
   #endregion
}
