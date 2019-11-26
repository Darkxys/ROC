// But : Programmation de l'outil qui va faire le pont avec la base de données.
// Auteur : Gabriel Duquette Godonm
// Date : 12 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
#endregion

public class DatabaseManager : MonoBehaviour
{
    #region Attributs
    private string host = "420.cstj.qc.ca";
    private string database = "rock_test";
    private string username = "ROCK";
    private string password = "Rock88811";

    public InputField IfUtilisateur;
    public InputField IfMotDePasse;

    public MySqlConnection con;
    public int userID;
    #endregion

    private void Awake()
    {
        ConnectBDD();
    }

    // Change avant le démarrage de l'application
    void ConnectBDD()
    {
        // Déclaration de la variable locale.
        string constr = "Server=" + host + ";DATABASE=" + database + ";User ID=" + username
           + ";Password=" + password + ";Pooling=true;Charset=utf8";

        try
        {
            con = new MySqlConnection(constr);

            // On ouvre la connection.
            con.Open();


        }
        catch (IOException Ex)
        {
            Debug.LogError(Ex);
        }
    }



    // Update is called once per frame
    void Update()
    {
    }

    #region Méthode publique
    public void Connection()
    {
        // On crée une variable locale.
        string temp_mot_de_passe = null;

        GameObject go = GameObject.FindGameObjectWithTag("Saver");
        if (go != null) 
            GameObject.Destroy(go);

        try
        {
            // On crée la commande sql.
            MySqlCommand commandesql = new MySqlCommand("SELECT * FROM utilisateurs WHERE NOM ='" + IfUtilisateur.text + "'", con);

            // On exécute la commande sql.
            MySqlDataReader MonLecteur;
            MonLecteur = commandesql.ExecuteReader();

            // On lis les données de l'utilisateur de la base de données.
            while (MonLecteur.Read())
            {
                // On stocke le mot de passe de l'utilisateur dans la variable "temp_mot_de_passe".
                temp_mot_de_passe = MonLecteur["MotDePasse"].ToString();

                // On vérie si le mot de passe est valide.
                if (temp_mot_de_passe == IfMotDePasse.text)
                {
                    userID = (int)MonLecteur["UtilisateurID"];

                    DontDestroyOnLoad(this);
                    SceneManager.LoadScene("MenuHeros", LoadSceneMode.Single);
                }
                else
                {
                    SceneManager.LoadScene("Notification_MotDePasse", LoadSceneMode.Single);
                }

            }

            // On ferme le lecture.
            MonLecteur.Close();
        }
        catch (IOException Ex)
        {
            Debug.LogError(Ex.ToString());
        }
    }


    public void Enregistrement()
    {
        // Déclaration des variables locales.
        bool existe = false;

        // On crée la commande vérification.
        MySqlCommand commande_Verification = new MySqlCommand("SELECT Nom FROM utilisateurs WHERE Nom='" + IfUtilisateur.text + "'", con);

        // On ouvre la lecture des données de la table de la base de données.
        MySqlDataReader MaLecture;
        MaLecture = commande_Verification.ExecuteReader();

        // Tant qui a des champs, fait ceci.
        while (MaLecture.Read())
        {
            if (MaLecture["Nom"].ToString() != "")
            {
                SceneManager.LoadScene("Notification_ExisteUtilisateur", LoadSceneMode.Single);
                existe = true;
            }
        }

        // S'il n'existe pas.
        if (!existe)
        {
            // On fait la requête.
            string requete = "INSERT INTO utilisateurs VALUES (default,'" + IfUtilisateur.text + "','" + IfMotDePasse.text + "')";
            MySqlCommand cmd = new MySqlCommand(requete, con);

            // Si la requête fonctionne pas, on envoie une erreur dans la console.
            try
            {
                cmd.ExecuteReader(); // envoie la requête a la base de données.

                SceneManager.LoadScene("Notification_Creation", LoadSceneMode.Single);
            }
            catch
            {
                Debug.LogError("Enregistrement pas fonctionner");
            }


            // On ferme la connection.
            cmd.Dispose();
        }

    }
    #endregion

    #region Méthode privées
    private void OnApplicationQuit()
    {
        Debug.Log("Fermeture connection");

        if (con != null && con.State.ToString() != "Closed")
        {
            con.Close();
        }
    }
    #endregion
}

