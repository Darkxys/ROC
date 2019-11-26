using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject _coffre;
    [SerializeField]
    private GameObject _ennemi;
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private InputField _inputLvl;
    [SerializeField]
    private Text _textLvl;
    [SerializeField]
    private Text _textCount;

    private Vector3 _lastPos;
    private System.Random _rnd = new System.Random(System.Environment.TickCount);
    private Dungeon dng;
    private int _level;
    private SaveHandler _handler;
    private int _scaling = 50;
    private int _scalingAfter = 20;

    // Start is called before the first frame update
    void Start()
    {
        _handler = GameObject.FindGameObjectWithTag("Saver").GetComponent<SaveHandler>();
        _level = _handler.levelDungeon;
        _textLvl.text += _level;
        List<TypeDonjon> lstType = GenerateTypes(_handler);

        int size = 200;
        if (5 + (_level - 1) > 12)
            size = 300;
        else if (5 + (_level - 1) > 9)
            size = 250;

        dng = new Dungeon(size,size, lstType[_rnd.Next(lstType.Count)], _level, 5 + (_level - 1));
        dng.GenerateDungeon();
        
        Vector3 posJoueur = dng.GetPlayerPos();

        GameObject lst = GameObject.FindGameObjectWithTag("lstEnnemy");

        InitPlayer(posJoueur);

        dng.GenerateEnnemies(_ennemi);

        GameObject coffre = Instantiate(_coffre);
        coffre.transform.position = dng.GetChestPos();

        coffre = Instantiate(_coffre);
        coffre.transform.position = posJoueur + new Vector3(1, 1, 0) * 4;

        _lastPos = _camera.transform.position;
        _textCount.text += dng.CountRooms();
    }

    private void InitPlayer(Vector3 pos)
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");
        player.transform.position = pos;

        PlayerStats stat = player.GetComponent<PlayerStats>();
        Hero hero = _handler.selectedSave.hero;
        List<int> lstItem = new List<int>();

        MySqlConnection con = _handler._dbHandler.con;
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        command.CommandText = "SELECT * FROM itemheros WHERE idHero = " + _handler.selectedSave.heroID;
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            int idItem = Int32.Parse(reader["idItem"].ToString());
            lstItem.Add(idItem);
        }
        reader.Close();

        stat.Init(hero.niveau, hero.exp, hero.gold, lstItem);
    }

    private List<TypeDonjon> GenerateTypes(SaveHandler handler)
    {
        List<TypeDonjon> lstType = new List<TypeDonjon>();

        MySqlConnection con = handler._dbHandler.con;
        MySqlDataReader reader;
        MySqlCommand command = con.CreateCommand();

        command.CommandText = "SELECT * FROM typesdonjon";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            TypeDonjon type = new TypeDonjon(reader);
            lstType.Add(type);
        }
        reader.Close();

        foreach (TypeDonjon type in lstType)
            type.GenerateRoom(handler._dbHandler.con);

        return lstType;
    }

    // Update is called once per frame
    void Update()
    {
        if(_lastPos != _camera.transform.position)
        {
            _lastPos = _camera.transform.position;

            dng.ShowEnnemies();
        }
    }

    public void EnnemyDead(GameObject go)
    {
        dng.EnnemyDead(go);
    }

    public void ChangeDungeonLevel()
    {
        _handler.levelDungeon = Int32.Parse(_inputLvl.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
