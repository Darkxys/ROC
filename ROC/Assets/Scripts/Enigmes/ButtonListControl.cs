using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField]
    private GameObject buttonTemplate;

    [SerializeField]
    private GameObject puzzle1;


    private void Start()
    {
        // Initialiser les valeurs des boutons

        // Initialisation statique (si la BD ne fonctionne pas)

        for (int i = 1; i <= 8; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject; 
            button.SetActive(true);

            button.GetComponent<ButtonListButton>().SetText("Énigme " + i, i);

            button.transform.SetParent(buttonTemplate.transform.parent, false);
        }



        /* Test rapide
        for (int i = 0; i <= 20; i++)
        {
            GameObject button = Instantiate(buttonTemplate) as GameObject;
            button.SetActive(true);

            button.GetComponent<ButtonListButton>().SetText("Button #" + i);

            button.transform.SetParent(buttonTemplate.transform.parent, false);

        }
        */
    }

    public void ButtonClicked(string myTextString, string id)
    {
        // REF des énigmes textuelles: https://www.cabaneaidees.com/15-enigmes-faciles-pour-les-enfants/
        CanvasGroup CanvaAOuvrir = GameObject.FindGameObjectWithTag("cnv1").GetComponent<CanvasGroup>();
        Text duTexte = GameObject.FindGameObjectWithTag("txtQuestion").GetComponent<Text>();
        Dropdown choixItem3 = GameObject.FindGameObjectWithTag("listeReponses").GetComponent<Dropdown>();
        Text txtBonus = GameObject.FindGameObjectWithTag("bonusEnigme").GetComponent<Text>();

        // Initialiser le titre des choix possibles
        choixItem3.options.Add(new Dropdown.OptionData() { text = "Réponses possibles" });
        choixItem3.value = choixItem3.options.Count - 1;
        choixItem3.options.RemoveAt(choixItem3.options.Count - 1);
        
        txtBonus.text = "Récompense : 30 exp.";

        duTexte.name = id;

        // Switch de la mort qui tue
        switch (id)
        {
            case "1":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "On peut me trouver au fond d’un bateau de pêche ou au milieu d’un court de tennis.";

                choixItem3.options[0].text = "Un filet";
                choixItem3.options[1].text = "Une serviette";
                choixItem3.options[2].text = "Un hameçon";

                break;
            case "2":

                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Je ne peux pas marcher, j’ai pourtant un dos et quatre pieds. Qui suis-je ?";

                choixItem3.options[0].text = "Une table";
                choixItem3.options[1].text = "Une chaise";
                choixItem3.options[2].text = "Une tortue";

                break;
            case "3":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Combien de gouttes d’eau peut-on mettre dans un verre vide?";

                choixItem3.options[0].text = "1";
                choixItem3.options[1].text = "250";
                choixItem3.options[2].text = "1000";
                break;
            case "4":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Trois poissons sont dans un seau, l’un d’entre meurt, combien en reste t-il?";

                choixItem3.options[0].text = "1";
                choixItem3.options[1].text = "2";
                choixItem3.options[2].text = "3";
                break;
            case "5":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Je ne fais pas de bruit quand je me réveille mais je réveille tout le monde.";

                choixItem3.options[0].text = "Le coq";
                choixItem3.options[1].text = "Le soleil";
                choixItem3.options[2].text = "Le froid";
                break;
            case "6":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Quand je suis frais je suis chaud.Qui suis-je ?";

                choixItem3.options[0].text = "De l'azote";
                choixItem3.options[1].text = "Du pain";
                choixItem3.options[2].text = "Le soleil";
                // Solution  :Le pain.

                break;
            case "7":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Qu’est ce qui est plus grand que la Tour Eiffel, mais infiniment moins lourd.";

                choixItem3.options[0].text = "Un ballon dirigeable";
                choixItem3.options[1].text = "La tour Eiffel sur la lune";
                choixItem3.options[2].text = "Son ombre";
                // Solution  :Son ombre

                break;
            case "8":
                CanvaAOuvrir.alpha = 1;
                CanvaAOuvrir.blocksRaycasts = true;
                duTexte.text = "Et enfin, je porte des lunettes mais je n’y vois rien. Qui suis-je ?";

                choixItem3.options[0].text = "Une taupe";
                choixItem3.options[1].text = "Un aveugle";
                choixItem3.options[2].text = "Un nez";
                // Solution  :le nez.

                break;
            case "9":
                CanvasGroup puzzle1 = GameObject.FindGameObjectWithTag("puzzle1").GetComponent<CanvasGroup>();
                puzzle1.alpha = 1;
                puzzle1.blocksRaycasts = true;



                break;

            default:
                break;
        }

    



    }

}
