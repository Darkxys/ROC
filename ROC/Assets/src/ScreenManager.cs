using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _menu;
    [SerializeField]
    private GameObject _menuCreate;
    [SerializeField]
    private GameObject _menuDelete;
    [SerializeField]
    private Text _nameHero;
    [SerializeField]
    private SaveHandler _saveHandler;

    public void Create()
    {
        SetInteractable(false);

        _menuCreate.SetActive(true);
    }

    public void Delete()
    {
        SetInteractable(false);

        _menuDelete.SetActive(true);

        _nameHero.text = _saveHandler.selectedSave.hero.nom;
    }

    public void Cancel()
    {
        _menuCreate.SetActive(false);
        _menuDelete.SetActive(false);

        SetInteractable(true);
    }

    private void SetInteractable(bool isInteractible)
    {
        foreach (Transform child in _menu.transform)
        {
            Button btn = child.gameObject.GetComponent<Button>();

            if (btn != null)
                btn.interactable = isInteractible;

        }
    }

}
