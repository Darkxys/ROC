using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Player : MonoBehaviour
{
    // Start is called before the first frame update
    [Space]
    [Header("CONST DU JOUEUR")]
    public Stat JoueurStats;
    public PlayerStats Joueur;
    public int niveau;
    public Text LevelText;
    public Text LevelTextCurr;
    public Text GoldText;

    [SerializeField] private GameObject _char;
    [SerializeField] private Game _game;
    [SerializeField] private RectTransform _minimap;
    [SerializeField] private GameObject _map;

    private void Awake()
    {
        JoueurStats.InitialiseVie();
        JoueurStats.InitialiseXp();
        niveau = Joueur.Level;
    }

    private void RefreshMiniMap()
    {
        RefreshPosMinimap(_minimap,_char);
    }

    public void RefreshPosMinimap(RectTransform _obj, GameObject _dynamic)
    {
        float size = _game.size;
        float sizeUI = (int)_obj.rect.width;
        float sizeUIH = (int)_obj.rect.height;

        float x = (sizeUI / size) * this.transform.position.x - sizeUI/2;
        float y = (sizeUIH / size) * this.transform.position.y - sizeUIH/2;

        _dynamic.transform.localPosition = new Vector3(x, y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        updateStats();
        RefreshMiniMap();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            JoueurStats.CurrentValVie += 10;

        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            JoueurStats.CurrentValVie -= 10;

        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            Joueur.Xp += 10;

        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Joueur.Xp -= 10;

        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Joueur.Gold += 50;

        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            _game.SetActiveEnnemies(false);
            _map.SetActive(true);
        }


        if (JoueurStats.CurrentValVie <= 0)
        {
            Destroy(gameObject);
        }
    }

    void updateStats()
    {

        JoueurStats.CurrentValXp = GetComponent<PlayerStats>().Xp;
        LevelText.text = (niveau + 1).ToString();
        LevelTextCurr.text = (niveau).ToString();
        JoueurStats.MaxValXp = (niveau + 1) * 50;
        JoueurStats.MaxValVie = 100;


        GoldText.text = (Joueur.Gold).ToString();

        niveau = Joueur.Level;

    }

}
