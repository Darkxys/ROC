using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Hero
{
    public int id;
    public string nom;
    public int niveau;
    public int exp;
    public int mana;
    public int gold;

    public Hero(MySqlDataReader reader)
    {
        id = Int32.Parse(reader["HerosID"].ToString());
        nom = reader["Nom"].ToString();
        niveau = Int32.Parse(reader["Niveau"].ToString());
        exp = Int32.Parse(reader["Exp"].ToString());
        mana = Int32.Parse(reader["Mana"].ToString());
        gold = Int32.Parse(reader["gold"].ToString());
    }

    public Hero(string name)
    {
        nom = name;
    }
}
