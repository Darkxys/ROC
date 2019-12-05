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
   [SerializeField] Animator canvasAnimation;
   [SerializeField] DataBase _db;
   [SerializeField] GameObject UtilisateurCreation;
   [SerializeField] GameObject QuitterBtn;
   EventSystem gestionnaire;
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion

   #region Méthode publique
   public void entrerMenuCreationCompte()                                                            
   {
      // On met la zone saisis d'utilisateur active.
      gestionnaire.SetSelectedGameObject(UtilisateurCreation);

      // On actionne l'animation de transition vers la fenêtre "Création Compte".
      canvasAnimation.SetTrigger("CreationCompte");
   }

   public void entrerMenuQuitter()
   {
      // On met le bouton quitter du menu "quitter" active.
      gestionnaire.SetSelectedGameObject(QuitterBtn);

      // On actionne l'animation pour aller au menu quitter.
      canvasAnimation.SetTrigger("MenuQuitter");
   }

   public void jouer()
   {
      DontDestroyOnLoad(_db);
      SceneManager.LoadScene("MenuHeros", LoadSceneMode.Single);
   }
   #endregion
}
