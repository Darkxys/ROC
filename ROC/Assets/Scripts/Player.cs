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

    private void Awake()
    {
        JoueurStats.InitialiseVie();
        JoueurStats.InitialiseXp();
        niveau = Joueur.Level;

        InvokeRepeating("updateStats", 1, 10f);

    }

    // Update is called once per frame
    void Update()
    {

       updateStats();

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

       


        if (JoueurStats.CurrentValVie <= 0 )
        {
            
            Destroy(gameObject);
        }
    }

   void updateStats()
   {

      JoueurStats.CurrentValXp = GetComponent<PlayerStats>().Xp;
      LevelText.text = (niveau+1).ToString();
      LevelTextCurr.text = (niveau).ToString();
      JoueurStats.MaxValXp = (niveau+1) * 50;
      JoueurStats.MaxValVie = 100;


        GoldText.text = (Joueur.Gold).ToString() ;

      niveau = Joueur.Level;

   }

}
