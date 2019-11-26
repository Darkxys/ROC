// But : Programmer la connection à la base de données de l'application
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

#region Bibliothèque
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

public abstract class DataBase : MonoBehaviour
{
   #region Attribut
   private string Host { get; set; }

    public Sauvegarde selectedSave;
    public SaveDB _dbHandler;
    public int levelDungeon = 1;

    [SerializeField]
    private GameObject _btnPrefab;
    [SerializeField]
    private GameObject _infoContainer;
    [SerializeField]
    private GameObject _btnContainer;
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private InputField _nameField;

    private List<Sauvegarde> _lstSave;
    private GameObject _infoActive;

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

    void Awake()
    {
        GameObject[] lst = GameObject.FindGameObjectsWithTag("ControllerSql");

        if (lst.Length > 1)
        {
            DataBase db = lst[1].GetComponent<DataBase>();
            _dbHandler = db._dbHandler;
            _lstSave = db._lstSave;

            GameObject.Destroy(lst[1]);
        }
        else 
        {
            _dbHandler = new SaveDB();
            _lstSave = new List<Sauvegarde>();
            InitList();
        }

        UpdateDataList();
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


    private void InitList()
    {
        MySqlDataReader reader;
        MySqlCommand command = _dbHandler.con.CreateCommand();

        command.CommandText = "SELECT * FROM sauvegardes WHERE idUtilisateur = '" + _dbHandler.userID.ToString() + "'";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            Sauvegarde save = new Sauvegarde(reader);
            _lstSave.Add(save);
        }
        reader.Close();

        foreach (Sauvegarde s in _lstSave)
        {
            MySqlDataReader readerHero;
            command = _dbHandler.con.CreateCommand();

            command.CommandText = "SELECT * FROM heros WHERE HerosID = '" + s.heroID + "'";
            readerHero = command.ExecuteReader();

            Hero hero = null;
            while (readerHero.Read())
                hero = new Hero(readerHero);

            s.hero = hero;
            readerHero.Close();
        }

    }

    public void UpdateDataList()
    {
        foreach (Transform child in _btnContainer.transform)
            GameObject.Destroy(child.gameObject);

        foreach (Sauvegarde save in _lstSave)
        {
            GameObject go = Instantiate(_btnPrefab);
            if (save == _lstSave[0])
                go.GetComponent<Button>().Select();


            go.transform.parent = _btnContainer.transform;
            go.transform.GetChild(0).GetComponent<Text>().text = "Nom : " + save.hero.nom + " | " + save.hero.niveau;
            go.GetComponent<BtnSave>().save = save;
        }

        if (_lstSave.Count > 0)
        {
            selectedSave = _lstSave[0];
            ShowInfoSave();
        }
    }

    private void OnApplicationQuit()
    {

    }

    public void Play()
    {
        DontDestroyOnLoad(this);
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }

    public void Return()
    {
        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }

    public void Delete()
    {
        int heroID = selectedSave.hero.id;

        DeleteSave();
        DeleteHero();

        _lstSave.Remove(selectedSave);

        selectedSave = null;

        UpdateDataList();
    }

    private void DeleteSave()
    {
        // Delete the save
        string requete = "DELETE FROM sauvegardes WHERE SauvegardeID = '" + selectedSave.id + "'";

        MySqlCommand cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();
    }

    private void DeleteHero()
    {
        // Delete the hero
        string requete = "DELETE FROM heros WHERE HerosID = '" + selectedSave.hero.id + "'";

        MySqlCommand cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();

        // Delete all competencehero
        requete = "DELETE FROM competencesheros WHERE idHero = '" + selectedSave.id + "'";

        cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();

        // Delete all itemshero
        requete = "DELETE FROM itemheros WHERE idHero = '" + selectedSave.id + "'";

        cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();
    }

    public void Create()
    {
        Hero newHero = CreateHero(_nameField.text);
        Sauvegarde newSave = CreateSauvegarde(newHero);
        newSave.hero = newHero;

        _lstSave.Add(newSave);

        UpdateDataList();
    }

    private Sauvegarde CreateSauvegarde(Hero hero)
    {
        string requete =
           "INSERT INTO sauvegardes " +
           "VALUES (default," + _dbHandler.userID + ",default," + hero.id + ")";

        MySqlCommand cmd = _dbHandler.con.CreateCommand();
        MySqlDataReader reader;
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();
        long idSauvegarde = cmd.LastInsertedId;

        requete = "SELECT * FROM sauvegardes WHERE SauvegardeID = '" + idSauvegarde + "'";

        cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        reader = cmd.ExecuteReader();
        Sauvegarde newSauvegarde = null;
        while (reader.Read())
            newSauvegarde = new Sauvegarde(reader);
        reader.Close();

        return newSauvegarde;
    }

    private Hero CreateHero(string name)
    {
        string requete =
            "INSERT INTO heros " +
            "VALUES (default,'" + name + "',default,default,default,default,default,default)";

        MySqlCommand cmd = _dbHandler.con.CreateCommand();
        MySqlDataReader reader;
        cmd.CommandText = requete;

        cmd.ExecuteNonQuery();
        long idHero = cmd.LastInsertedId;

        requete = "SELECT * FROM heros WHERE HerosID = '" + idHero + "'";

        cmd = _dbHandler.con.CreateCommand();
        cmd.CommandText = requete;

        reader = cmd.ExecuteReader();
        Hero newHero = null;
        while (reader.Read())
            newHero = new Hero(reader);
        reader.Close();

        return newHero;
    }

    public void ShowInfoSave()
    {
        if (_infoActive != null)
            GameObject.Destroy(_infoActive);

        _infoActive = Instantiate(_infoContainer);

        _infoActive.name = "InfoHero";
        _infoActive.transform.position = _menu.transform.position;
        _infoActive.transform.parent = _menu.transform;

        Hero h = selectedSave.hero;
        string[] info = { h.nom, h.niveau.ToString(), h.exp.ToString(), selectedSave.levelMax.ToString() };

        for (int i = 0; i < info.Length; i++)
            _infoActive.transform.GetChild(i).GetComponent<Text>().text += info[i];

        levelDungeon = selectedSave.levelMax;
    }

    public void SaveGame()
    {
        GameObject go = GameObject.FindGameObjectWithTag("player");
        if (!go) return;

        MySqlConnection con = _dbHandler.con;
        MySqlDataReader reader;
        Player player = go.GetComponent<Player>();
        PlayerStats statsPlayer = player.Joueur;
        Stat stat = player.JoueurStats;

        int exp = statsPlayer.Xp;
        int attaque = 10 * statsPlayer.Level;
        int level = statsPlayer.Level;
        int gold = statsPlayer.Gold;
        int vie = (int)stat.MaxValVie;
        List<int> lstItem = statsPlayer.ItemsPos;

        // Update on Hero table
        string request =
            "UPDATE heros " +
            "SET Niveau = " + level + ", Exp = " + exp + ", gold = " + gold + ", vie = " + vie + ", attaque = " + attaque + " " +
            "WHERE HerosID = " + selectedSave.heroID;

        MySqlCommand cmd = con.CreateCommand();
        cmd.CommandText = request;

        cmd.ExecuteNonQuery();

        // Update on all itemshero table
        request =
            "SELECT * FROM itemheros WHERE idHero = " + selectedSave.heroID;

        cmd = con.CreateCommand();
        cmd.CommandText = request;

        reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            foreach (int id in statsPlayer.ItemsPos)
            {
                if (id == Int32.Parse(reader["idHero"].ToString()))
                {
                    lstItem.Remove(id);
                    break;
                }
            }
        }
        reader.Close();

        foreach (int id in lstItem)
        {
            request =
            "INSERT INTO itemheros" +
            "VALUES (" + id + ", " + selectedSave.heroID + ")";

            cmd = con.CreateCommand();
            cmd.CommandText = request;

            cmd.ExecuteNonQuery();
        }
    }
}
