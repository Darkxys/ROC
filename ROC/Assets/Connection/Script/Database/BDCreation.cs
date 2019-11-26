// But : Programmation du module de création de compte dans la base de donnée
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

public class BDCreation : DataBase
{
   #region Attribut
   private InputField Nom { get; set; }
   
   private InputField MotDePasse { get; set; }
   
   private InputField ConfirmationMotDePasse { get; set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On initialise les attributs.
      Nom = GameObject.FindGameObjectWithTag("CreationUtilisateur").GetComponent<InputField>();
      MotDePasse = GameObject.FindGameObjectWithTag("CreationMotPasse").GetComponent<InputField>();
      ConfirmationMotDePasse = GameObject.FindGameObjectWithTag("CreationConfirmation").GetComponent<InputField>();
   }
   #endregion

   #region Méthode publique
   public string CreationCompte()
   {
      // On vérifie si les deux mot passe ne sont pas pareil
      if (MotDePasse.text != ConfirmationMotDePasse.text)
      {
         Debug.Log("Mot de passe pas pareil");

         // On retourne au mot clée pour dire que les mot de passe sont différent.
         return "MotPasseDifferentCreation";
      }
      else
      {
         // On ouvre la connection.
         ConnectionBd();

         // On crée la commande de vérification.
         MySqlCommand verification = new MySqlCommand("SELECT Nom FROM utilisateurs WHERE Nom='" + Nom.text + "'", Connecteur);

         // On ouvre la lecture des données de la table de la base de données.
         MySqlDataReader Malecture;
         Malecture = verification.ExecuteReader();

         // Tant qui a des champs dans la lecture, fait ceci.
         while (Malecture.Read())
         {
            // Si l'utilisateur existe déjà, fait ceci.
            if (Malecture["Nom"].ToString() != "")
            {
               Debug.Log("L'utilisateur existe déjà");

               // On ferme la requête.
               Malecture.Close();

               // On ferme la connection.
               Connecteur.Close();

               // On retourne un mot clée pour dire que l'utilisateur existe déjà.
               return "UtilisateurExiste";
            }
         }

         // On ferme la requête.
         Malecture.Close();

         // On fait la requête.
         string requete = "INSERT INTO utilisateurs VALUES (default,'" + Nom.text + "','" + MotDePasse.text + "')";
         MySqlCommand commandeInsertion = new MySqlCommand(requete, Connecteur);

         // Si la requête fonctionne pas, on envoie une erreur dans la console
         try
         {
            // On envoie la requête a la base données.
            commandeInsertion.ExecuteReader();
         }
         catch
         {
            Debug.LogError("Insertion n'a pas fonctionner dans la base de données");
         }

         // On ferme la commande.
         commandeInsertion.Dispose();

         // On ferme la connection.
         Connecteur.Close();

         // On dit que l'utilisateur a été créer.
         return "CreationCompteReussi";
      }
   }
   #endregion
}
