// But : Programmer les options avancées du menu.
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuPrincipal : MonoBehaviour
{
   #region Attribut
   [SerializeField] Button btnFermuture;
   [SerializeField] Animator canvasAnimation;
   [SerializeField] DataBase _db;
   [SerializeField] GameObject UtilisateurCreation;
   EventSystem gestionnaire;
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion

   #region Méthode publique

   /// <summary>
   /// Permettre à l’utilisateur de fermer l’application par l’option.
   /// </summary>
   public void fermetureApplication()
   {
      Application.Quit();
   }

   public void entrerMenuCreationCompte()
   {
      // On met la zone saisis d'utilisateur active.
      gestionnaire.SetSelectedGameObject(UtilisateurCreation);

      // On actionne l'animation de transition vers la fenêtre "Création Compte".
      canvasAnimation.SetTrigger("CreationCompte");
   }


   public void jouer()
   {
      DontDestroyOnLoad(_db);
      SceneManager.LoadScene("MenuHeros", LoadSceneMode.Single);
   }
   #endregion
}
