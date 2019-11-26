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

   private string Utilisateur { get; set; }

   private string MotDePasse { get; set; }

   protected MySqlConnection Connecteur { get; private set; }
   #endregion

   #region Constante
   const string NOM_FICHIER_CONFIG = "Config.txt";
   #endregion

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
}
