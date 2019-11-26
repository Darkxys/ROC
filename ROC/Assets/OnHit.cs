using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{

    [Space]
    [Header("Constantes Pour flèches")]
    public int DEGATS_BASE_FLECHES = 10;

    public int NIVEAU_DU_JOUEUR;

    public GameObject Joueur;
    public GameObject effet;

    private void Awake()
    {
        Joueur = GameObject.FindGameObjectWithTag("player");
        NIVEAU_DU_JOUEUR = Joueur.GetComponent<Player>().Niveau;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

            Ennemy unEnnemi = collision.gameObject.GetComponent<Ennemy>();
        if (unEnnemi)
        {
            unEnnemi.vie -= DEGATS_BASE_FLECHES * NIVEAU_DU_JOUEUR; 
        }
        GameObject effetG = Instantiate(effet, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(effetG, 1f);

    }
}
