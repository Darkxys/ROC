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
        NIVEAU_DU_JOUEUR = Joueur.GetComponent<Player>().niveau;

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

            Ennemy unEnnemi = collision.gameObject.GetComponent<Ennemy>();
        if (unEnnemi)
        {
            unEnnemi.rb.isKinematic = true;
            unEnnemi.vie -= DEGATS_BASE_FLECHES * NIVEAU_DU_JOUEUR;
            unEnnemi.rb.isKinematic = false;
        }
        GameObject effetG = Instantiate(effet, transform.position, Quaternion.identity);
        Destroy(gameObject);
        Destroy(effetG, 1f);

    }
}
