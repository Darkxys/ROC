using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;
using System.Linq;
using UnityEngine.UI;

public class Dungeon
{
    const int _sizeRoom = 25;
    const int _sizeRoomBoss = 45;

    private Generator _dungeonGenerator;
    private Tilemap _ground = new Tilemap();
    private Tilemap _collision = new Tilemap();
    private Tilemap _air = new Tilemap();
    private bool[,] _dungeonCollision;
    private Dictionary<Vector3, GameObject> _lstEnnemy;
    private Dictionary<Vector3, GameObject> _lstEnnemyInstantiate;
    private TypeDonjon _dungeonType;
    private int _level;

    public Dungeon(int _widthDungeon, int _heightDungeon, TypeDonjon type, int lvl, int qtt)
    {
        InitLayers();
        _dungeonType = type;
        _level = lvl;
        _lstEnnemy = new Dictionary<Vector3, GameObject>();
        _lstEnnemyInstantiate = new Dictionary<Vector3, GameObject>();

        _dungeonGenerator = new Generator(_widthDungeon, _heightDungeon,_dungeonType,qtt);
    }

    public int CountRooms() 
    { 
        return _dungeonGenerator.CountRooms(); 
    }

    public void GenerateDungeon()
    {
        _dungeonGenerator.Generate();

        // des qu on voit 1, mettre l'index d'un plancher
        int[,] dungeonGround = _dungeonGenerator.GenerateGround();

        // des qu'on voit qqc autre que 0, c'est l'index de la tile
        int[,] dungeonCollision = _dungeonGenerator.GenerateCollision();

        // des qu'on voit pas 0, index de la tile
        int[,] dungeonAir = _dungeonGenerator.GenerateAir();

        // des qu'on voit pas 0, index resource/typename/index.png
        int[,] dungeonRoomTile = _dungeonGenerator.GenerateRoomCollision();

        GenerateTiles(dungeonGround, dungeonCollision, dungeonAir, dungeonRoomTile);
    }

    private void GenerateTiles(int[,] ground, int[,] collision, int[,] air, int[,] collisionRoom)
    {
        Tile aTile = new Tile();
        Sprite[] allSprites = Resources.LoadAll<Sprite>("Tiles/" + _dungeonType.nom.ToString());
        Sprite[] allSpritesRoom = Resources.LoadAll<Sprite>("TypeTiles/" + _dungeonType.nom.ToString());
        List<int[,]> lstOof = new List<int[,]>();

        lstOof.Add(ground);
        lstOof.Add(air);
        lstOof.Add(collisionRoom);

        for (int i = 0; i < ground.GetLength(0); i++)
        {
            for (int j = 0; j < ground.GetLength(1); j++)
            {
                foreach(int[,] arr in lstOof)
                {
                    if(arr == ground)
                    {
                        if (arr[i, j] == 0)
                        {
                            aTile.sprite = allSpritesRoom[21];
                            aTile.name = ground[i, j].ToString();
                            _ground.SetTile(new Vector3Int(i, j, 0), aTile);
                        }
                    }

                    if (arr[i, j] != 0)
                    {
                        aTile.sprite = allSpritesRoom[arr[i, j] - 1];
                        aTile.name = arr[i, j].ToString();

                        if(arr == ground)
                            _ground.SetTile(new Vector3Int(i, j, 0), aTile);
                        else if (arr == air)
                            _air.SetTile(new Vector3Int(i, j, 0), aTile);
                        else if (arr == collisionRoom)
                            _collision.SetTile(new Vector3Int(i, j, 0), aTile);
                    }
                }

                if (collision[i, j] != 0)
                {
                    aTile.sprite = allSprites[collision[i, j] - 1];
                    aTile.name = collision[i, j].ToString();
                    _collision.SetTile(new Vector3Int(i, j, 0), aTile);
                }
            }
        }
    }

    public void EnnemyDead(GameObject go)
    {
        Vector3 pos = Vector3.zero;

        foreach (KeyValuePair<Vector3, GameObject> ennemy in _lstEnnemyInstantiate)
        {
            if (ennemy.Value == go) pos = ennemy.Key;
        }

        _lstEnnemyInstantiate.Remove(pos);
        _lstEnnemy.Remove(pos);
    }

    public Vector3 GetPlayerPos()
    {
        return _dungeonGenerator.GetPosStart();
    }

    public Vector3 GetChestPos()
    {
        return _dungeonGenerator.GetChestPos();
    }

    public void GenerateEnnemies(GameObject theEnnemy)
    {
        List<Vector3> lstEnnemy = _dungeonGenerator.GetPointsOfSpawn();
        Transform parent = GameObject.FindGameObjectWithTag("lstEnnemy").transform;
        GameObject ennemy = InstanciateEnnemy(theEnnemy);
        Sprite sprite = Resources.Load<Sprite>("Ennemies/" + _dungeonType.nomEnemy);

        foreach (Vector3 position in lstEnnemy)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Vector3 pos = position - new Vector3(x, y, 0);

                    GameObject go = GameObject.Instantiate(ennemy);
                    go.transform.position = pos;
                    go.transform.parent = parent;
                    go.name = _dungeonType.nomEnemy;
                    go.SetActive(false);
                    go.GetComponent<SpriteRenderer>().sprite = sprite;

                    _lstEnnemy.Add(pos, go);
                }
            }
        }

        GameObject go1 = GameObject.Instantiate(ennemy);
        go1.transform.position = _dungeonGenerator.GetPosBoss();
        go1.transform.parent = parent;
        go1.name = "BOSS";
        go1.GetComponent<Ennemy>().vie *= 10;
        go1.transform.localScale = new Vector3(5, 5, 1);
        go1.GetComponent<SpriteRenderer>().sprite = sprite;
        go1.SetActive(false);

        _lstEnnemy.Add(go1.transform.position, go1);

        GameObject.Destroy(ennemy);
    }

    public void ShowEnnemies()
    {
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 screenOrigo = Camera.main.ScreenToWorldPoint(Vector2.zero);
        Dictionary<Vector3, GameObject> lst = new Dictionary<Vector3, GameObject>();

        foreach (KeyValuePair<Vector3, GameObject> ennemy in _lstEnnemy)
        {
            Vector3 pos = ennemy.Key;

            if ((pos.x > screenBounds.x || pos.x < screenOrigo.x || pos.y > screenBounds.y || pos.y < screenOrigo.y))
            {
                if (_lstEnnemyInstantiate.ContainsKey(pos))
                {
                    GameObject go = _lstEnnemyInstantiate[pos];

                    _lstEnnemyInstantiate.Remove(pos);
                    pos = go.transform.position;

                    go.SetActive(false);
                }
            }
            else if (!_lstEnnemyInstantiate.ContainsKey(pos))
            {
                _lstEnnemyInstantiate.Add(pos, ennemy.Value);
                ennemy.Value.SetActive(true);
            }

            lst.Add(pos, ennemy.Value);
        }

        _lstEnnemy = lst;
    }

    private GameObject InstanciateEnnemy(GameObject go)
    {
        GameObject ennemy = GameObject.Instantiate(go);

        ennemy.name = ennemy.GetComponent<Ennemy>().nom;
        ennemy.GetComponent<Ennemy>().level = _level;
        ennemy.transform.parent = GameObject.FindGameObjectWithTag("lstEnnemy").transform;

        return ennemy;
    }

    private void InitLayers()
    {
        _ground = GameObject.FindGameObjectWithTag("layerGround").GetComponent<Tilemap>();
        _collision = GameObject.FindGameObjectWithTag("layerCollision").GetComponent<Tilemap>();
        _air = GameObject.FindGameObjectWithTag("layerAir").GetComponent<Tilemap>();
    }
}
