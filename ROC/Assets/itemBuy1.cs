using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 

public class itemBuy1 : MonoBehaviour
{
    private PlayerStats Joueur;

    private void Start()
    {
        Joueur = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>();
        if (Joueur.Gold - 20 >= 0)
        {
            Joueur.Gold -= 20;
            //Joueur.AttaqueBonnus += 5;
        }
    }

}
