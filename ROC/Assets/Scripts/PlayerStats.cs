using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ce script contient toutes les valeurs utilisé pour le joueur qui sont stoqué dans la BD

public class PlayerStats : MonoBehaviour
{
    //-------Liste des variables -------

    [Space]
    [Header("Joueur")]
    private int xp;
    private int gold;
    private List<int> itemsPos;
    private int level = 1;

    public int Level { get => level; set => level = value; }
    public List<int> ItemsPos { get => itemsPos; set => itemsPos = value; }
    public int Gold { get => gold; set => gold = value; }
    public int Xp { get => xp; set => xp = value; }





    //----------------------------------

    //-------Awake pour lier les données avec la BD --------

    private void Awake()
    {
        Level = 1;
        Xp = 0; ;
        Gold = 0;
        ItemsPos = new List<int>();

        InvokeRepeating("updateStats", 1, 10f);
    }

    public void Init(int lvl, int _xp, int _gold, List<int> lstItems)
    {
        level = lvl;
        xp = _xp;
        gold = _gold;
        itemsPos = lstItems;
        //InvokeRepeating("UpdateDMG", 1, 10f);
    }

    //------------------------------------------------------




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        levelXp();
    }

    //---------Sauvegarder dans la BD ------------------

    //--------------------------------------------------
    void levelXp()
    {

        if (Xp > (Level + 1) * 50)
        {
            Xp = Xp - (Level + 1) * 50;
            Level++;
        }

    }



  
}

