using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnEnigme : MonoBehaviour
{
    public Enigme enigme;
    public ButtonListControl btnControl;

    public void ShowEnigme()
    {
        btnControl.enigmeSelectionee = enigme;
    }

}
