// But : Programmer le module de la connexion de la base de données.
// Auteur : Gabriel Duquette Godon
// Date : 19 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

using UnityEngine;
using UnityEngine.UI;
#endregion

public class BDConnexion : DataBase
{
   #region Attribut
   private InputField NomField { get; set; }

   private InputField PasswordField { get; set; }
   #endregion

   #region Méthode Unity
   private void Awake()
   {
      // On initialise les attributs.
      NomField = GameObject.FindGameObjectWithTag("UtilisateurConnection").GetComponent<InputField>();
      PasswordField = GameObject.FindGameObjectWithTag("MotDePasseConnexion").GetComponent<InputField>();
   }
   #endregion

   #region Méthode publique
   public string Connexion()
   {
      // On fait la connection à la base de données.
      ConnectionBd();

      // On crée une variable temporaire pour le mot de passe.
      string tempMotDePasse = null;

      if (NomField.text == "" && PasswordField.text == "")
      {
         // On ferme la connection.
         Connecteur.Close();

         // On envoie une notification dans le debug.
         Debug.Log("Veuillez entrer les informations de connexion.");

         // On renvoie qui n'a pas d'information.
         return "pasInformation";
      }
      else
      {
         // On crée la commande Sql.
         MySqlCommand commandeSql = new MySqlCommand("SELECT * FROM utilisateurs WHERE NOM ='" + NomField.text + "'", Connecteur);

         // on exécute la commande sql,
         MySqlDataReader MonLecteur;
         MonLecteur = commandeSql.ExecuteReader();

         // On lis les données de l'utilisateur de la base de données.
         while (MonLecteur.Read())
         {
            // On stocke le mot de passe de l'utilisateur dans la variable "tempMotDePasse".
            tempMotDePasse = MonLecteur["motdepasse"].ToString();

            // On vérifie si le mot de passe est valide.
            if (tempMotDePasse == PasswordField.text)
            {
               // On envoie une notification dans le debugeur.
               Debug.Log("Connexion est réussite.");

               // On ferme la commande sql.
               MonLecteur.Close();

               // On ferme la connexion.
               Connecteur.Close();

               // On renvoie que la connexion a réussi.
               return "ConnexionReussi";
            }
            else
            {
               // On envoie une notification dans le debugeur.
               Debug.Log("Mauvais mot de passe.");

               // On femre la commande sql.
               MonLecteur.Close();

               // On ferme la connexion.
               Connecteur.Close();

               // On renvoie que le mot de passe n'est pas valide.
               return "mauvaisMotDePasse";
            }
         }

         // On ferme la commande sql.
         MonLecteur.Close();
      }

      // On ferme la connection.
      Connecteur.Close();

      // On met un message d'erreur absolue si sa dépasse toute la fonction.
      return "Poney";
   }
   #endregion
}
