// But : Programmer la connection à la base de données de l'application
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#endregion

public class DataBase : MonoBehaviour
{
   #region Attribut

   #region Chemin du fichier connection
   const string NOM_FICHIER_CONFIG = "Config.txt";
   #endregion

   #region Connecteur
   string host;
   string database;
   string username;
   string password;
   public MySqlConnection connection;
   #endregion

   [SerializeField] InputField nomField;
   [SerializeField] InputField passwordField;

   [SerializeField] InputField nomCreationField;
   [SerializeField] InputField passwordCreationField;
   [SerializeField] InputField confirmationMotDePasseField;

   [SerializeField] NConnexion fenetrenotification;
   [SerializeField] MenuPrincipal fenetreMenuPrincipal;

   public int userID;
   #endregion                

   #region Méthode Unité
   private void Awake()
   {
      // Si le fichier existe on le lis, sinon il faut le créee.
      if (!File.Exists(Application.dataPath + "/../" + NOM_FICHIER_CONFIG))
         CreeFichierConfig();

      // On lis les informations du fichier de configuration.
      LireFichier();
   }
   #endregion

   #region Méthode privé
   private void ConnectionBd()
   {
      // Déclaration de la variable locale.
      string constr = "server=" + host + ";uid=" + username + ";pwd=" + password + ";database=" + database + ";";

      try
      {
         connection = new MySqlConnection(constr);

         // On ouvre la connection.
         connection.Open();


      }
      catch (IOException Ex)
      {
         Debug.LogError(Ex);
      }
   }

   private void CreeFichierConfig()
   {
      // On crée le fichier
      StreamWriter fichier = new StreamWriter(Application.dataPath + "/../" + NOM_FICHIER_CONFIG);

      // On écrit les informations par défaut du fichier.
      fichier.Write("host=420.cstj.qc.ca\r");
      fichier.Write("database=rock_test\r");
      fichier.Write("username=ROCK\r");
      fichier.Write("password=Rock88811\r");

      // On ferme le fichier.
      fichier.Close();
   }

   private void LireFichier()
   {
      try
      {
         // On ouvre le fichier de config.
         using (StreamReader streamReader = new StreamReader(Application.dataPath + "/../" + NOM_FICHIER_CONFIG))
         {
            // On prend le contenu.
            string contenu = streamReader.ReadToEnd();

            // On crée une liste avec les lignes du documents.
            List<string> lignes = new List<string>(contenu.Split("\r"[0]));

            for (int index = 0; index < lignes.Count(); index++)
            {
               // On trouve la position du égale.
               int position = lignes[index].IndexOf("=");

               switch (index)
               {
                  // On extrait l'information du host
                  case 0:
                     host = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom de table de la base de données.
                  case 1:
                     database = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom d'utilisateur de la base de données.
                  case 2:
                     username = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du mot de passe de la base de données.
                  case 3:
                     password = lignes[index].Substring(position + 1);
                     break;
               }
            }
         }
      }
      catch (IOException e)
      {
         Debug.LogError("Le fichier ne peut pas ouvrir.");
         Debug.LogError(e.Message);
      }
   }

   #endregion

   #region Méthode publique
   public string CreationCompte()
   {
      // On ouvre la connection.
      ConnectionBd();

      // On crée la commande de vérification.
      MySqlCommand verification = new MySqlCommand("SELECT Nom FROM utilisateurs WHERE Nom='" + nomCreationField.text + "'", connection);

      // On ouvre la lecture des données de la table de la base de données.
      MySqlDataReader Malecture;
      Malecture = verification.ExecuteReader();

      // Tant qui a des champs dans la lecture, fait ceci.
      while (Malecture.Read())
      {
         if (Malecture["Nom"].ToString() != "")
         {
            // On ferme la requête.
            Malecture.Close();

            // On ferme la connection.
            connection.Close();

            // On dit que l'utilisateur existe.
            return "Utilisateur existe";
         }
      }

      // On ferme la requête.
      Malecture.Close();

      // On fait la requête pour crée l'utilisateur.
      string requete = "INSERT INTO utilisateurs VALUES (default,'" + nomCreationField.text + "','" + passwordCreationField.text + "')";
      MySqlCommand commandeInsertion = new MySqlCommand(requete, connection);

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
      connection.Close();

      // On envoie le code de réussite.
      return "Réussi";
   }


   public void Connection()
   {
      // On fait la connection à la base de données.
      ConnectionBd();

      // On crée une variable temporaire pour le mot de passe.
      string tempMotDePasse = null;

      if (nomField.text == "" && passwordField.text == "")
         fenetrenotification.chargementinfoNotification("pasInformation");
      else
      {
         // On crée la commande Sql.
         MySqlCommand commandeSql = new MySqlCommand("SELECT * FROM utilisateurs WHERE NOM ='" + nomField.text + "'", connection);

         // on exécute la commande sql,
         MySqlDataReader MonLecteur;
         MonLecteur = commandeSql.ExecuteReader();

         // On lis les données de l'utilisateur de la base de données.
         while (MonLecteur.Read())
         {
            // On stocke le mot de passe de l'utilisateur dans la variable "tempMotDePasse".
            tempMotDePasse = MonLecteur["motdepasse"].ToString();

            // On vérifie si le mot de passe est valide.
            if (tempMotDePasse == passwordField.text)
            {
               userID = (int)MonLecteur["UtilisateurID"];
               DontDestroyOnLoad(this);
               SceneManager.LoadScene("MenuHeros", LoadSceneMode.Single);

            }
            else
               fenetrenotification.chargementinfoNotification("mauvaisMotDePasse");
         }

         // On ferme la commande sql.
         MonLecteur.Close();
      }

      // On ferme la connection.
      connection.Close();
   }

   #endregion
}
