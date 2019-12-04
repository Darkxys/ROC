// But : Programmer les options avancées du menu.
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuPrincipal : MonoBehaviour
{
    #region Attribut
    [SerializeField] Button btnFermuture;
    [SerializeField] Animator canvasAnimation;
    [SerializeField] DataBase _db;
    #endregion

    #region Méthode publique

    /// <summary>
    /// Permettre à l’utilisateur de fermer l’application par l’option.
    /// </summary>
    public void fermetureApplication()
    {
        Application.Quit();
    }

    public void jouer()
    {
        DontDestroyOnLoad(_db);
        SceneManager.LoadScene("MenuHeros", LoadSceneMode.Single);
    }
    #endregion
}
