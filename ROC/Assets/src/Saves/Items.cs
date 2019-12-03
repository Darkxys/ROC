using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Items
{

    private string nom;
    private int id;
    private int vie;
    private int armure;
    private int attaque;
    private int niveau;
    private int prix;

    public string Nom { get => nom; set => nom = value; }
    public int Vie { get => vie; set => vie = value; }
    public int Armure { get => armure; set => armure = value; }
    public int Attaque { get => attaque; set => attaque = value; }
    public int Niveau { get => niveau; set => niveau = value; }
    public int Prix { get => prix; set => prix = value; }
    public int Id { get => id; set => id = value; }

    public Items(MySqlDataReader reader)
    {
        Id = Int32.Parse(reader["ItemID"].ToString());
        nom = reader["Nom"].ToString();
        niveau = Int32.Parse(reader["Niveau"].ToString());
        vie = Int32.Parse(reader["Vie"].ToString());
        armure = Int32.Parse(reader["Armure"].ToString());
        attaque = Int32.Parse(reader["Attaque"].ToString());
        prix = Int32.Parse(reader["Prix"].ToString());
    }
}