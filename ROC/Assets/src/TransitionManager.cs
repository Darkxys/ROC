using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuTransit;
    [SerializeField] private GameObject _menuSave;
    [SerializeField] private Game _game;

    public void Save()
    {
        _menuSave.SetActive(false);
        SetInteractable(true);
    }

    public void ShowSave()
    {
        _menuSave.SetActive(true);
        SetInteractable(false);
    }

    private void SetInteractable(bool isInteractible)
    {
        if (!_menuTransit.activeSelf) return;

        foreach (Transform child in _menuTransit.transform)
        {
            Button btn = child.gameObject.GetComponent<Button>();

            if (btn != null)
                btn.interactable = isInteractible;

        }
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
    }

    public void AllerBase()
    {
        _game.TPBase();
        _menuTransit.SetActive(false);
    }
}
