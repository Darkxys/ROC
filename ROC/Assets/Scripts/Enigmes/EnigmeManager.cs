using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

public class EnigmeManager : MonoBehaviour
{
    public Game leJeu;

    private List<Enigme> lstEnigme;
    private SaveDB sauv;

    // Use this for initialization
    void Start()
    {
        // Initialisation lstEnigme
        sauv = leJeu._handler._dbHandler;

        InitListe();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void InitListe()
    {
        List<Enigme> lstEni = new List<Enigme>();

        MySqlConnection con = sauv.con;
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        command.CommandText = "SELECT * FROM enigmes";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            Enigme eni = new Enigme(reader);
            lstEni.Add(eni);
        }

        reader.Close();
    }
}
