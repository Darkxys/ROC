using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnSave : MonoBehaviour
{
    public Sauvegarde save;
    private SaveHandler _saveHandler;

    private void Awake()
    {
        _saveHandler = GameObject.FindGameObjectWithTag("Saver").GetComponent<SaveHandler>();
    }

    public void ShowInfoSave()
    {
        _saveHandler.selectedSave = save;
        _saveHandler.ShowInfoSave();
    }
}
