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
   #region Zone texte
   [SerializeField] InputField Utilisateur;
   [SerializeField] InputField MotDePasse;
   [SerializeField] InputField Confirmation_MotDePasse;
   #endregion

   #region Constances
   private const int MIN_CARACTERE = 1;
   private const int MAX_CARACTERE = 15;
   #endregion

   #region Système
   EventSystem systeme;
   [SerializeField] DataBase manager_bd;
   #endregion

   NCreation notification;

   #endregion

   private void Awake()
   {
      // On initialise la variable system.
      systeme = EventSystem.current;
      notification = GameObject.FindGameObjectWithTag("NoCreation").GetComponent<NCreation>();
   }

   #region Méthode publique
   public void Creation()
   {
      // Déclaration de la variable locale.
      string reponse;
      
      // Si les champs sont vide, tu fait ceci.
      if (Utilisateur.text == "" && MotDePasse.text == "" && Confirmation_MotDePasse.text == "")
         notification.chargementinfoNotification("pasInformation");

      // Si l'utilisateur ne respecte pas les normes du nombre de caractère, envoie lui une notification.
      if(Utilisateur.text.Length < MIN_CARACTERE || Utilisateur.text.Length > MAX_CARACTERE)
         notification.chargementinfoNotification("GrandeurIncorrecte");

      // Si les deux mot de passe sont différent fait ceci.
      if (MotDePasse.text != Confirmation_MotDePasse.text)
         notification.chargementinfoNotification("MotPasseDifferentCreation");

      // On crée l'utilisateur et on reçois la réponse de la base de donnée.
      reponse = manager_bd.CreationCompte();

      // On analyse la réponse de la base de donnée.
      switch(reponse)
      {
         case "Réussi":
            notification.chargementinfoNotification("CreationCompteReussi");
            break;

         case "Utilisateur existe":
            notification.chargementinfoNotification("UtilisateurExiste");
            break;
      }
   }
   #endregion
}
