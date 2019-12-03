using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Enigme
{
    public int id;
    public int niveau;
    public string nom;
    public string question;
    public string reponse;
    public string choix1;
    public string choix2;
    public string choix3;

    public Enigme(int idE, int niveauE, string nomE, string questionE, string reponseE, string choix1E, string choix2E, string choix3E)
    {
        id = idE;
        niveau = niveauE;
        nom = nomE;
        question = questionE;
        reponse = reponseE;
        choix1 = choix1E;
        choix2 = choix2E;
        choix3 = choix3E;
    }

    public Enigme(MySqlDataReader reader, int id) : this(id,
                                                Int32.Parse(reader["niveau"].ToString()),
                                                reader["nom"].ToString(),
                                                reader["question"].ToString(),
                                                reader["reponse"].ToString(),
                                                reader["choix1"].ToString(),
                                                reader["choix2"].ToString(),
                                                reader["choix3"].ToString()){ }
}