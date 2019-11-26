using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

public class Sauvegarde
{
    public int id;
    public int userID;
    public int levelMax;
    public int heroID;
    public Hero hero;

    public Sauvegarde(string _id,string _userID,string _levelMax,string _heroID)
    {
        id = Int32.Parse(_id);
        userID = Int32.Parse(_userID);
        levelMax = Int32.Parse(_levelMax);
        heroID = Int32.Parse(_heroID);
    }

    public Sauvegarde(MySqlDataReader arr) : this(arr["sauvegardeID"].ToString(), 
        arr["idUtilisateur"].ToString(), arr["levelMax"].ToString(),arr["heroID"].ToString()){ }
}
