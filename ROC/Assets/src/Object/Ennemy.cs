using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Ennemy : MonoBehaviour
{
    public string nom;
    public int vie;
    public int attaque;
    public int level = 1;

    private float _speed = 4f;
    private int _aggroRange = 10;
    public Rigidbody2D rb;
    private Vector3 _offset;

    private GameObject player;
    private Tilemap tlm;
    public GameObject mortEffet;
    private List<Vector2Int> path;
    private PlayerStats playerStats;

    private void Awake()
    {
        vie = 50 * level;
        playerStats = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>();
        player = GameObject.FindGameObjectWithTag("player");
        tlm = GameObject.FindGameObjectWithTag("layerGround").GetComponent<Tilemap>();
        _offset = Vector3.zero;

        path = new List<Vector2Int>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        InvokeRepeating("Updating", 1, 2f);
    }

    // Update is called once per frame
    private void Updating()
    {
        if (player)
        {
            if (Vector3.Distance(player.transform.position, transform.position) > 5)
            {
                Vector3 toPosition = player.transform.position;
                Vector3 direction = toPosition - transform.position;

                Vector3Int ennemiPlayer = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);

                float dist = Vector2Int.Distance(new Vector2Int(ennemiPlayer.x, ennemiPlayer.y), new Vector2Int((int)toPosition.x, (int)toPosition.y));

                if (dist < _aggroRange && dist > 1)
                {
                    _offset = ennemiPlayer - new Vector3Int(1, 1, 0) * _aggroRange;

                    int[,] map = TransformTilemap(ennemiPlayer, tlm);

                    Vector3 posPlayer = player.transform.position - _offset;
                    Vector2 pos2D = posPlayer;

                    map = AddCornerPlayer(pos2D, map);

                    Vector2Int plz = new Vector2Int((int)pos2D.x, (int)pos2D.y);
                    path = PathFinding.FindPath(new Vector2Int(_aggroRange, _aggroRange), plz, map);
                }
            }
        }

    }

    private void Update()
    {
        if (player)
        {
            if (path.Count > 0)
            {
                Vector3 posPath = new Vector3(path[path.Count - 1].x + _offset.x, path[path.Count - 1].y + _offset.y, 0);
                Vector3 dir = posPath - transform.position;
                dir.Normalize();
                Debug.DrawLine(posPath, transform.position);

                rb.velocity = dir * _speed;

                if (Vector3.Distance(transform.position, posPath) < 1)
                    path.RemoveAt(path.Count - 1);
            }
            else if (Vector3.Distance(player.transform.position, transform.position) <= 5)
            {
                Vector3 posPath = player.transform.position;
                Vector3 dir = posPath - transform.position;
                dir.Normalize();
                Debug.DrawLine(posPath, transform.position);

                rb.velocity = dir * _speed;
            }
        }

        if (vie <= 0)
        {
            Game gm = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
            playerStats.Xp += Random.Range(level + 5, level + 8);
            playerStats.Gold += Random.Range(level + 10, level + 15);
            gm.EnnemyDead(gameObject);

            Destroy(gameObject);
            Instantiate(mortEffet, transform.position, Quaternion.identity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player Joueur = collision.gameObject.GetComponent<Player>();
        if (Joueur)
        {
            Joueur.JoueurStats.CurrentValVie -= 10;
        }
    }

    private int[,] TransformTilemap(Vector3Int pos, Tilemap tlm)
    {
        int[,] map = new int[_aggroRange * 2 + 1, _aggroRange * 2 + 1];
        int initPos = -1 * _aggroRange;

        for (int i = initPos; i <= _aggroRange; i++)
        {
            for (int j = initPos; j <= _aggroRange; j++)
            {
                if (i + pos.x < 0 || i + pos.x >= tlm.size.x || j + pos.y < 0 || j + pos.y >= tlm.size.y)
                {
                    map[i + _aggroRange, j + _aggroRange] = 1;
                    continue;
                }

                Vector3Int thePos = pos + new Vector3Int(i, j, 0);
                TileBase aTile = tlm.GetTile(thePos);

                if (aTile == null) map[i + _aggroRange, j + _aggroRange] = 1;
                else map[i + _aggroRange, j + _aggroRange] = 0;
            }
        }

        return map;
    }

    private int[,] AddCornerPlayer(Vector2 pos, int[,] map)
    {
        int x = (int)pos.x;
        int y = (int)pos.y;

        for (int i = -1; i < 2; i += 2)
        {
            for (int j = -1; j < 2; j += 2)
            {
                if (i + x < 0 || i + x >= map.GetLength(0) || j + y < 0 || j + y >= map.GetLength(1))
                    continue;

                map[x + i, y + j] = 1;
            }
        }

        return map;
    }
}
