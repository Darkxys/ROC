using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveHandler : MonoBehaviour
{
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
    void Awake()
    {
        _dbHandler = new SaveDB();
        _lstSave = new List<Sauvegarde>();
        InitList();

        UpdateDataList();
    }

   

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
            foreach(int id in statsPlayer.ItemsPos)
            {
                if (id == Int32.Parse(reader["idHero"].ToString()))
                {
                    lstItem.Remove(id);
                    break;
                }
            }
        }
        reader.Close();

        foreach(int id in lstItem)
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
