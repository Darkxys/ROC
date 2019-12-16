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
   DataBase _db;
   [SerializeField] GameObject UtilisateurCreation;
   [SerializeField] GameObject QuitterBtn;
   [SerializeField] GameObject controllerSauvegarde;
   [SerializeField] NConnexion notification;

   InputField nomField;
   InputField passwordField;

   EventSystem gestionnaire;
   #endregion

   #region Méthode Unité

   private void Awake()
   {
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
      _db = GameObject.FindGameObjectWithTag("Connector").GetComponent<DataBase>();
      nomField = GameObject.FindGameObjectWithTag("UtilisateurConnection").GetComponent<InputField>();
      passwordField = GameObject.FindGameObjectWithTag("UtilisateurMotPasse").GetComponent<InputField>();
   }

   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.KeypadEnter))
      {
         connexion();
      }
   }
   #endregion

   #region Méthode publique
   public void connexion()
   {
      string connexion = _db.Connection(nomField.text, passwordField.text);

      // On efface les info du nom et du mot de passe.
      nomField.text = passwordField.text = "";

      switch(connexion)
      {
         case "pasInformation":
            notification.chargementinfoNotification(connexion);
            break;

         case "mauvaisMotDePasse":
            notification.chargementinfoNotification(connexion);
            break;

         case "Jouer":
            controllerSauvegarde.SetActive(true);

            if (controllerSauvegarde.GetComponent<SaveHandler>()._dbHandler.con.State.ToString() == "Closed")
               controllerSauvegarde.GetComponent<SaveHandler>().changerList();

            canvasAnimation.SetTrigger("MenuHero");
            break;
      }
   }

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
   #endregion
}
