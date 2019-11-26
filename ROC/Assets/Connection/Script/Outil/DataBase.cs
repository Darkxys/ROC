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
#endregion

public abstract class DataBase : MonoBehaviour
{
   #region Attribut
   private string Host { get; set; }
   
   private string BD { get; set; }

<<<<<<< HEAD
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
=======
   private string Utilisateur { get; set; }
>>>>>>> 898d48c7889b76f1ae33cacf0203a950cadc44da

   private string MotDePasse { get; set; }

   protected MySqlConnection Connecteur { get; private set; }
   #endregion

<<<<<<< HEAD
   [SerializeField] Notification fenetrenotification;
   [SerializeField] MenuPrincipal fenetreMenuPrincipal;

    public int userID;
   #endregion                
=======
   #region Constante
   const string NOM_FICHIER_CONFIG = "Config.txt";
   #endregion
>>>>>>> 898d48c7889b76f1ae33cacf0203a950cadc44da

   #region Méthode Unité
   private void OnEnable()
   {
      // Si le fichier existe on le lis, sinon il faut le créee.
      if (!File.Exists(Application.dataPath + "/../" + NOM_FICHIER_CONFIG))
      {
         Debug.Log("Creation du fichier " + NOM_FICHIER_CONFIG + ",");

         CreeFichierConfig();
      }
      else
         Debug.Log("Le fichier " + NOM_FICHIER_CONFIG + " existe !");

      // On lis les informations du fichier de configuration.
      LireFichier();
   }
   #endregion

   #region Méthode protégé
   protected void ConnectionBd()
   {
      // Déclaration de la variable locale.
      string constr = "server=" + Host + ";uid=" + Utilisateur + ";pwd=" + MotDePasse + ";database=" + BD + ";";

      try
      {
         Connecteur = new MySqlConnection(constr);

         // On ouvre la connection.
         Connecteur.Open();
      }
      catch (IOException Ex)
      {
         Debug.LogError(Ex);
      }
   }
   #endregion

   #region Méthode privé

   private void CreeFichierConfig()
   {
      Debug.Log("Création du fichier " + NOM_FICHIER_CONFIG + ".");

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
                     Host = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom de table de la base de données.
                  case 1:
                     BD = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom d'utilisateur de la base de données.
                  case 2:
                     Utilisateur = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du mot de passe de la base de données.
                  case 3:
                     MotDePasse = lignes[index].Substring(position + 1);
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
<<<<<<< HEAD

   #region Méthode publique
   public void CreationCompte()
   {
      if (passwordCreationField.text != confirmationMotDePasseField.text)
      {
         Debug.Log("Mot de passe pas pareil");

         fenetrenotification.chargementinfoNotification("MotPasseDifferentCreation");
      }
      else
      {
         // Déclaration de la variable locale.
         bool existe = false;

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
               Debug.Log("L'utilisateur existe déjà");

               // On génère la fenêtre de notification.
               fenetrenotification.chargementinfoNotification("UtilisateurExiste");

               // On dit que l'utilisateur existe.
               existe = true;
            }
         }

         // On ferme la requête.
         Malecture.Close();

         // Si l'utilisateur n'existe pas, fait ceci.
         if (!existe)
         {
            // On fait la requête.
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

            // On envoie la notification de création de compte.
            fenetrenotification.chargementinfoNotification("CreationCompteReussi");
         }

         // On ferme la connection.
         connection.Close();
      }
   }


   public void Connection()
   {
      // On fait la connection à la base de données.
      ConnectionBd();

      // On crée une variable temporaire pour le mot de passe.
      string tempMotDePasse = null;

      if (nomField.text == "" && passwordField.text == "")
      {
         Debug.Log("Veuillez entrer les informations de connexion.");
         fenetrenotification.chargementinfoNotification("pasInformation");
      }
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
               Debug.Log("Connexion est réussite.");
               fenetreMenuPrincipal.entrerDansMenuPrincipal();

            }
            else
            {
               Debug.Log("Mauvais mot de passe.");
               fenetrenotification.chargementinfoNotification("mauvaisMotDePasse");
            }
         }

         // On ferme la commande sql.
         MonLecteur.Close();
      }

      // On ferme la connection.
      connection.Close();
   }

   #endregion
=======
>>>>>>> 898d48c7889b76f1ae33cacf0203a950cadc44da
}
