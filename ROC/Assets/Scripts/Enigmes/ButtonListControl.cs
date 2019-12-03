using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private EnigmeManager _enigmeManager;

    public Enigme enigmeSelectionee;

    private List<Enigme> _liste;

    private PlayerStats player;

    public int compteur = 0;

    public const int EXP_GAGNE = 30;

    public List<GameObject> _boutonClone;


    public void Start()
    {
        updateListeEnigme();
    }

    public void updateListeEnigme()
    {
        _liste = _enigmeManager.LstEnigme;

        foreach (GameObject clone in _boutonClone)
        {
            Destroy(clone);
        }

        foreach (Enigme itemEnigme in _liste)
        {
          
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);
            button.GetComponent<ButtonListButton>().SetText(("  " + (itemEnigme.id + 1).ToString() + "." + itemEnigme.nom.ToString()), itemEnigme.id);
            button.tag = "btnClone";

            button.transform.SetParent(buttonTemplate.transform.parent, false);

            _boutonClone.Add(button);

        }

    }

    public void mettreEnigme()
    {
       

    }

    public void ButtonClicked(string myTextString, int id)
    {
        // REF des énigmes textuelles: https://www.cabaneaidees.com/15-enigmes-faciles-pour-les-enfants/

        // Mettre dans le START
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("cnv1").GetComponent<CanvasGroup>();
        Text duTexte = GameObject.FindGameObjectWithTag("txtQuestion").GetComponent<Text>();
        Dropdown choixItem3 = GameObject.FindGameObjectWithTag("listeReponses").GetComponent<Dropdown>();
        Text txtBonus = GameObject.FindGameObjectWithTag("bonusEnigme").GetComponent<Text>();

        // Initialiser le titre des choix possibles
        choixItem3.options.Add(new Dropdown.OptionData() { text = "Réponses possibles" });
        choixItem3.value = choixItem3.options.Count - 1;
        choixItem3.options.RemoveAt(choixItem3.options.Count - 1);

        txtBonus.text = "Récompense : 30 exp.";
        
        enigmeSelectionee = _liste[id];

        duTexte.name = enigmeSelectionee.id.ToString();

        CanvaAOuvrir.alpha = 1;
        CanvaAOuvrir.blocksRaycasts = true;

        duTexte.text = enigmeSelectionee.question;

        choixItem3.options[0].text = enigmeSelectionee.choix1;
        choixItem3.options[1].text = enigmeSelectionee.choix2;
        choixItem3.options[2].text = enigmeSelectionee.choix3;

        /*
            CanvasGroup puzzle1 = GameObject.FindGameObjectWithTag("puzzle1").GetComponent<CanvasGroup>();
            puzzle1.alpha = 1;
            puzzle1.blocksRaycasts = true;
        */

    }

    public void verifierChoixReponse()
    {
        Text duTexte = GameObject.FindGameObjectWithTag("txtQuestion").GetComponent<Text>();
        Dropdown choixItem3 = GameObject.FindGameObjectWithTag("listeReponses").GetComponent<Dropdown>();
        CanvasGroup CanMauvais = GameObject.FindGameObjectWithTag("txtMauvais").GetComponent<CanvasGroup>();

        player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>();

        if (choixItem3.options[choixItem3.value].text == "Réponses possibles") { CanMauvais.alpha = 0; return; }

        validerChoix(duTexte.name, choixItem3.options[choixItem3.value].text);
    }

    public void validerChoix(string noEnigme, string choix)
    {
        Text textFelicitation = GameObject.FindGameObjectWithTag("txtFelicitation").GetComponent<Text>();
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("felicitation").GetComponent<CanvasGroup>();
        CanvasGroup CanMauvais = GameObject.FindGameObjectWithTag("txtMauvais").GetComponent<CanvasGroup>();
        CanvasGroup messageEchec = GameObject.FindGameObjectWithTag("echec").GetComponent<CanvasGroup>();
        Text txtBonus = GameObject.FindGameObjectWithTag("bonusEnigme").GetComponent<Text>();

        // if (Validation(noEnigme, choix))
        if (_liste[Int32.Parse(noEnigme)].reponse == choix)
        {
            if (compteur == 0)
            {
                textFelicitation.text = "Bravo vous avez résolue l'énigme !\n\n Récompense obtenue : " + (EXP_GAGNE).ToString() + " exp";
                player.Xp += EXP_GAGNE;
                compteur = 0;
            }
            else if (compteur == 1)
            {
                textFelicitation.text = "Bravo vous avez résolue l'énigme !\n\n Récompense obtenue : " + (EXP_GAGNE - 20).ToString() + " exp";
                player.Xp += (EXP_GAGNE - 20);
                compteur = 0;
            }

            CanMauvais.alpha = 0;
            txtBonus.text = "";
            CanvaAOuvrir.alpha = 1;
            CanvaAOuvrir.blocksRaycasts = true;

        }
        else
        {
            if (compteur >= 1)
            {
                CanMauvais.alpha = 0;
                messageEchec.alpha = 1;
                messageEchec.blocksRaycasts = true;
                compteur = 0;

            }
            else
            {
                txtBonus.text = "Récompense : 10 exp.";
                CanMauvais.alpha = 1;
                compteur += 1;
            }
        }
    }

}
