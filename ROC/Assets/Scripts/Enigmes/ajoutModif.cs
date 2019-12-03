using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ajoutModif : MonoBehaviour
{
    [SerializeField]
    private ButtonListControl _btnControl;

    [SerializeField]
    private EnigmeManager _enigmeManager;

    [SerializeField]
    private GameObject ajouterEnigme;

    [SerializeField]
    private GameObject mofifierEnigme;

    [SerializeField]
    private Text titre;

    [SerializeField]
    private InputField inputNom;

    [SerializeField]
    private InputField inputQuestion;

    [SerializeField]
    private InputField inputReponse;

    [SerializeField]
    private InputField inputChoix1;

    [SerializeField]
    private InputField inputChoix2;

    [SerializeField]
    private Text messageErreur;

    public void ouvertureAjouterEnigme()
    {
        titre.text = "Ajouter une énigme";
        ajouterEnigme.SetActive(true);
    }

    public void ouvertureModifierEnigme()
    {
        titre.text = "Modifier l'énigme";
        ajouterEnigme.SetActive(true);
    }

    public void ajouterNouvelleEnigme()
    {
        if (verifierAjout())
        {
            messageErreur.text = "";

            _enigmeManager.AjouterEnigme(inputNom.text, 1, inputQuestion.text, inputReponse.text, inputChoix1.text, inputChoix2.text);

            ajouterEnigme.SetActive(false);
        }
    }

    public void modifierEnigme()
    {
        Enigme monEnigme = _btnControl.enigmeSelectionee;

        inputNom.text = monEnigme.nom;
        


    }

    public bool verifierAjout()
    {
        messageErreur.text = "";

        if (inputNom.text.Length < 2)
        {
            messageErreur.text = "Le nom est trop court (2 caractères minimum).";
            return false;

        }
        else if(inputQuestion.text.Length < 10)
        {
            messageErreur.text = "La question est trop courte (10 caractères minimum).";
            return false;
        }
        else if (inputReponse.text.Length < 10)
        {
            messageErreur.text = "La réponse est trop courte (10 caractères minimum).";
            return false;
        }

        return true;
    }

}
