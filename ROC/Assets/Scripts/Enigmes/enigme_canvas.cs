using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class enigme_canvas : MonoBehaviour
{
    [SerializeField]
    private Button buttontest;

    [SerializeField]
    private GameObject reglage;

    public int compteur = 0;
    public const int EXP_GAGNE = 30;
    private PlayerStats player;



    public void OnClickAbandonner()
    {
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("cnv1").GetComponent<CanvasGroup>();
        CanvasGroup canFelicitation = GameObject.FindGameObjectWithTag("felicitation").GetComponent<CanvasGroup>();
       // CanvasGroup puzzle1 = GameObject.FindGameObjectWithTag("puzzle1").GetComponent<CanvasGroup>();

        CanvaAOuvrir.alpha = 0;
        CanvaAOuvrir.blocksRaycasts = false;
        canFelicitation.alpha = 0;
        canFelicitation.blocksRaycasts = false;
      //  puzzle1.alpha = 0;
      //  puzzle1.blocksRaycasts = false;


        reglage.transform.gameObject.SetActive(false);
    }

    public void fermerEchec()
    {
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("cnv1").GetComponent<CanvasGroup>();
        CanvasGroup messageEchec = GameObject.FindGameObjectWithTag("echec").GetComponent<CanvasGroup>();
        messageEchec.alpha = 0;
        messageEchec.blocksRaycasts = false;
        CanvaAOuvrir.alpha = 0;
        CanvaAOuvrir.blocksRaycasts = false;


    }

    /// <summary>
    /// Validation "harcodée"
    /// </summary>
    /// <param name="enigme"></param>
    /// <param name="reponse"></param>
    /// <returns></returns>
    public bool Validation(string enigme, string reponse)
    {
        if (enigme == "1") return (reponse == "Un filet") ? true : false;
        if (enigme == "2") return (reponse == "Une chaise") ? true : false;
        if (enigme == "3") return (reponse == "1") ? true : false;
        if (enigme == "4") return (reponse == "3") ? true : false;
        if (enigme == "5") return (reponse == "Le soleil") ? true : false;
        if (enigme == "6") return (reponse == "Du pain") ? true : false;
        if (enigme == "7") return (reponse == "Son ombre") ? true : false;
        if (enigme == "8") return (reponse == "Un nez") ? true : false;
    
        return false;
    }

    public void validerChoix(string noEnigme, string choix) 
    {
        Text textFelicitation = GameObject.FindGameObjectWithTag("txtFelicitation").GetComponent<Text>();
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("felicitation").GetComponent<CanvasGroup>();
        CanvasGroup CanMauvais = GameObject.FindGameObjectWithTag("txtMauvais").GetComponent<CanvasGroup>();
        CanvasGroup messageEchec = GameObject.FindGameObjectWithTag("echec").GetComponent<CanvasGroup>();
        Text txtBonus = GameObject.FindGameObjectWithTag("bonusEnigme").GetComponent<Text>();

        if (Validation(noEnigme, choix))
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
            txtBonus.text = "Récompense : 10 exp.";
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

    public void OnClickVerifier()
    {
        Text duTexte = GameObject.FindGameObjectWithTag("txtQuestion").GetComponent<Text>();
        Dropdown choixItem3 = GameObject.FindGameObjectWithTag("listeReponses").GetComponent<Dropdown>();
        CanvasGroup CanMauvais = GameObject.FindGameObjectWithTag("txtMauvais").GetComponent<CanvasGroup>();

        player = GameObject.FindGameObjectWithTag("player").GetComponent<PlayerStats>();

        if (choixItem3.options[choixItem3.value].text == "Réponses possibles") { CanMauvais.alpha = 0; return; }

        switch (duTexte.name)
        {
            case "1":
                validerChoix("1", choixItem3.options[choixItem3.value].text);
                break;
            case "2":
                validerChoix("2", choixItem3.options[choixItem3.value].text);
                break;
            case "3":
                validerChoix("3", choixItem3.options[choixItem3.value].text);
                break;
            case "4":
                validerChoix("4", choixItem3.options[choixItem3.value].text);
                break;
            case "5":
                validerChoix("5", choixItem3.options[choixItem3.value].text);
                break;
            case "6":
                validerChoix("6", choixItem3.options[choixItem3.value].text);
                break;
            case "7":
                validerChoix("7", choixItem3.options[choixItem3.value].text);
                break;
            case "8":
                validerChoix("8", choixItem3.options[choixItem3.value].text);
                break;
            default:
                break;
        }


    }
}
