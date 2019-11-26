// But : Programmer la fenêtre de connection pour accéder au jeu.
// Auteur : Gabriel Duquette Godon
// Date : 9 octobre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using MySql;   // On rajoute la bibliothèque MySql.

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#endregion

public class Connection : MonoBehaviour
{
    #region Attributs
    EventSystem systeme;

    public Button btnConnection;
    public Button btnCreationCompte;
    public Button btnFermer;

    public InputField Utilisateur;
    public InputField MotDePasse;
    #endregion

    #region Méthode Unity

    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        systeme = EventSystem.current;
    }

    private void Update()
    {
        // On fait la tabulation si l'utilisateur clique sur tabulation.
        Tabulation();
    }
    #endregion

    #region Méthode publique

    /// <summary>
    /// Jeu de test pour essayer la connection
    /// </summary>
    public void Verification_Saisis_Debug()
    {
        if (Utilisateur.text == "" && MotDePasse.text == "")
            SceneManager.LoadScene("Notification_Default", LoadSceneMode.Single);
        else
        {
            // Si la zone saisis n'a pas le texte "Équipe", envoie une erreur dans la console. 
            if (Utilisateur.text != "Équipe")
                SceneManager.LoadScene("Notification_Utilisateur", LoadSceneMode.Single);
            else
            {
                // On vérifie si le mot de passe est valide.
                if (MotDePasse.text != "Mario334!")
                    SceneManager.LoadScene("Notification_MotDePasse", LoadSceneMode.Single);
                else
                    Debug.Log("Connection réussite");
            }
        }
    }

    public bool Verification_Nom(string nom)
    {


        return true;
    }

    public void Creation_Compte()
    {
        SceneManager.LoadScene("CreationCompte", LoadSceneMode.Single);
    }
    #endregion

    #region Méthode privé
    public void Fermer_Application()
    {
        Debug.Log("Fermeture Application");
        Application.Quit();
    }

    private void Tabulation()
    {
        // Si l'utilisateur clique sur la tabulation, fait ceci.
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // On crée une variable qui va sélectionner la prochain objet.
            Selectable prochain = systeme.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            // Si le prochain objet n'est pas null, fait ceci.
            if (prochain != null)
            {
                // On crée une variable bouton qui va trouver le prochain bouton.
                InputField zonedetexte = prochain.GetComponent<InputField>();

                // Si le prochain bouton n'est pas null, fait ceci.
                if (zonedetexte != null)
                    zonedetexte.OnPointerClick(new PointerEventData(systeme));

                systeme.SetSelectedGameObject(prochain.gameObject, new BaseEventData(systeme));
            }
        }
    }
    #endregion

}