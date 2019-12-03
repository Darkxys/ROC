using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class EnigmeManager : MonoBehaviour
{
   public Game leJeu;

    private List<Enigme> lstEnigme;
    private SaveDB sauv;

    public List<Enigme> LstEnigme {
        get
        {
            return lstEnigme;
        }
    }

    // Use this for initialization
    void Start()
    {
        // Initialisation lstEnigme
        sauv = leJeu._handler._dbHandler;

        InitListe();
    }

    private void InitListe()
    {
        int i = 0;
        lstEnigme = new List<Enigme>();

        MySqlConnection con = sauv.con;
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        command.CommandText = "SELECT * FROM enigmes";
        reader = command.ExecuteReader();

        while (reader.Read())
        {

            Enigme eni = new Enigme(reader, i);
            lstEnigme.Add(eni);
            i++;
        }

        reader.Close();
    }

    public void AjouterEnigme(string nom, int niveau, string question, string reponse, string choix1, string choix2)
    {
        int i = lstEnigme.Count;

        MySqlConnection con = sauv.con;
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        command.CommandText = "INSERT INTO enigmes " +
                              "VALUES (default, " + niveau.ToString() + ", " + nom + ", " + question + ", " + reponse + ", " + choix1 + ", " + choix2 + ", " + reponse + ", default)";
        reader = command.ExecuteReader();

        reader.Close();

        InitListe();
    }

}
