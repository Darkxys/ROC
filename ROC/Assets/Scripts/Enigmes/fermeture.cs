using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class fermeture : MonoBehaviour
{
    [SerializeField]
    private GameObject menuReglage;

    [SerializeField]
    private GameObject editionEnigme;

    [SerializeField]
    private GameObject ajouterEnigme;

    [SerializeField]
    private GameObject confirmation;

    public void fermerAjouterEnigme() { confirmation.SetActive(false); menuReglage.SetActive(false); ajouterEnigme.SetActive(false); } 

    public void fermerEditionEnigme() { menuReglage.SetActive(false); editionEnigme.SetActive(false); }

}
