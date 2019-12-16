using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public SaveHandler _handler;
    public int size;

    [SerializeField] private GameObject _coffre;
    [SerializeField] private GameObject _ennemi;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputField _inputLvl;
    [SerializeField] private Text _textLvl;
    [SerializeField] private Text _textCount;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private Tilemap _coll;
    [SerializeField] private Tilemap _air;
    [SerializeField] private GameObject _gridBase;
    [SerializeField] private GameObject _interfaceTransition;
    [SerializeField] private GameObject _cameraMinimap;

    private Vector3 _lastPos;
    private System.Random _rnd = new System.Random(System.Environment.TickCount);
    private Dungeon dng;
    private int _level;
    private int _scaling = 50;
    private int _scalingAfter = 20;
    private GameObject player;
    private bool _ennemiesCanSpawn = true;

    // Start is called before the first frame update
    void Awake()
    {
        List<Tilemap> lstMap = new List<Tilemap>();
        lstMap.Add(_ground);
        lstMap.Add(_coll);
        lstMap.Add(_air);


        _handler = GameObject.FindGameObjectWithTag("Saver").GetComponent<SaveHandler>();
        _level = _handler.levelDungeon;
        _textLvl.text += _level;
        List<TypeDonjon> lstType = GenerateTypes(_handler);

        size = 200;
        if (5 + (_level - 1) > 12)
            size = 300;
        else if (5 + (_level - 1) > 9)
            size = 250;

        dng = new Dungeon(size,size, lstType[_rnd.Next(lstType.Count)], _level, 5 + (_level - 1), lstMap);
        dng.GenerateDungeon();
        
        Vector3 posJoueur = dng.GetPlayerPos();

        GameObject lst = GameObject.FindGameObjectWithTag("lstEnnemy");

        InitPlayer(posJoueur);

        dng.GenerateEnnemies(_ennemi, _interfaceTransition);

        GameObject coffre = Instantiate(_coffre);
        coffre.transform.position = dng.GetChestPos();

        coffre = Instantiate(_coffre);
        coffre.transform.position = posJoueur + new Vector3(1, 1, 0) * 4;

        _lastPos = _camera.transform.position;
        _textCount.text += dng.CountRooms();

        _cameraMinimap.transform.position = new Vector3(size / 2, size / 2, 0);
        _cameraMinimap.GetComponent<Camera>().orthographicSize = size / 2;
    }

    private void InitPlayer(Vector3 pos)
    {
        player = GameObject.FindGameObjectWithTag("player");
        player.transform.position = pos;

        PlayerStats stat = player.GetComponent<PlayerStats>();
        Stat stats = player.GetComponent<Player>().JoueurStats;
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
        stats.CurrentValVie = hero.vie;
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
        if(_lastPos != _camera.transform.position && _ennemiesCanSpawn)
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

    public void TPBase()
    {
        player.transform.position = _gridBase.transform.position;
    }

    public void NextDungeon()
    {
        _handler.selectedSave.levelMax += 1;
        _handler.levelDungeon += 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetActiveEnnemies(bool active)
    {
        if (active) 
            dng.ContinueEnnemies();
        else 
            dng.StopEnnemies();

        _ennemiesCanSpawn = active;
    }

    public bool CanTP(int x, int y)
    {
        return dng.CanTP(x,y);
    }
}
