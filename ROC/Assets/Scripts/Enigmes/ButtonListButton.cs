using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListButton : MonoBehaviour
{
    [SerializeField]
    private Text myText;
    [SerializeField]
    private ButtonListControl buttonControl;

    [SerializeField]
    private GameObject coffre;



    private string myTextString;
    private string idButton;

    public void SetText(string textString, int id)
    {
        myText.text = textString;
        myText.name = id.ToString();

        myTextString = textString;
        idButton = id.ToString();
    }

    public void OnClick()
    {
        // Quand le bouton est cliqué : (executer les fonctions)

        buttonControl.ButtonClicked(myTextString, idButton);
    }

    public void OnClickCoffre()
    {
        System.Random r = new System.Random();
        int rand = r.Next(1, 8);
        coffre.SetActive(false);

        buttonControl.ButtonClicked("Énigme du coffre", rand.ToString());

    }
}
