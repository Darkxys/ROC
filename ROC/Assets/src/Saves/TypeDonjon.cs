using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TypeDonjon
{
    public int id;
    public string nom;
    public List<int> idsSalle;
    // index 0 : Depart | index 1 : boss | index > 1 : milieu
    public List<Salle> lstSalle;
    public string nomEnemy;

    public TypeDonjon(int _id,string name, int idStart,int idMiddle,int idBoss)
    {
        idsSalle = new List<int>();
        lstSalle = new List<Salle>();
        id = _id;
        nom = name;
        idsSalle.Add(idStart);
        idsSalle.Add(idBoss);
        idsSalle.Add(idMiddle);
    }

    public TypeDonjon(MySqlDataReader reader) : this(Int32.Parse(reader["idTypeDonjon"].ToString()), reader["nom"].ToString(), 
        Int32.Parse(reader["idSalleDepart"].ToString()), Int32.Parse(reader["idSalleMilieu"].ToString()),Int32.Parse(reader["idSalleBoss"].ToString())) { }

    public void GenerateRoom(MySqlConnection con)
    {
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        string requete = "SELECT * FROM salles WHERE idSalle = " + idsSalle[0];
        for (int i = 1; i < idsSalle.Count; i++)
            requete += " OR idSalle = " + idsSalle[i];

        command.CommandText = requete;
        reader = command.ExecuteReader();
        List<Salle> lstTmp = new List<Salle>();

        while (reader.Read())
        {
            foreach (int idSalle in idsSalle)
            {
                if(idSalle == Int32.Parse(reader["idSalle"].ToString()))
                {
                    Salle salle = new Salle(reader);
                    lstTmp.Add(salle);

                    break;
                }
            }
        }
        reader.Close();

        foreach(int id in idsSalle)
        {
            foreach (Salle salle in lstTmp)
            {
                if (salle.id == id)
                {
                    lstSalle.Add(salle);
                    break;
                }
            }
        }

        command = con.CreateCommand();

        requete = "SELECT * FROM ennemis WHERE idTypeDonjon = " + id;

        command.CommandText = requete;
        reader = command.ExecuteReader();
        Debug.Log("oof");
        while (reader.Read())
        {
            Debug.Log("oof1");
            nomEnemy = reader["Nom"].ToString();
            Debug.Log(nomEnemy);
        }
        reader.Close();

    }
}