// But : Programmer le menu d'héros
// Auteur : Gabriel Duquette Godon
// Date : 10 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;
#endregion

public class MenuHero : MonoBehaviour
{
   #region Attribut
   EventSystem gestionnaire;
   Animator canvasAnimation;
   public GameObject db;
   [SerializeField] NHeros notification;
   [SerializeField] InputField niveauField;
   [SerializeField] SaveHandler controllerSauvegarde;
   [SerializeField] GameObject utilisateurConnexion;
   [SerializeField] GameObject zoneTexteSauvegarde;
   #endregion

   #region Méthode unité
   private void Awake()
   {
      // On initialise les attributs.
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique
   public void entrerCreationSauvegarde()
   {
      // On sélectionne la zone texte pour entrer le nom de la sauvegarde.
      gestionnaire.SetSelectedGameObject(zoneTexteSauvegarde);

      // On envoie l'utilisateur dans le menu de création de sauvegarde.
      canvasAnimation.SetTrigger("MenuSauvegarde");
   }

   public void jouer()
   {
      // Si la zone de saisis des niveau n'est pas vide, fait ceci.
      if (niveauField.text != "")
      {
         int niveau = int.Parse(niveauField.text);

         if (niveau > 0 && niveau <= 30)
         {
            // On met le niveau dans la variable "levelDungeon".
            controllerSauvegarde.levelDungeon = niveau;

            // On actionne le téchargement du jeu.
            StartCoroutine(telechargerScene());
         }
         else
         {
            notification.chargementinfoNotification("NiveauInvalide");
         }

      }
      else
      {
         notification.chargementinfoNotification("pasInformation");
      }


   }

   IEnumerator telechargerScene()
   {
      // Définir la scène actuelle pour pouvoir la décharger plus tard.
      Scene sceneCourrant = SceneManager.GetActiveScene();

      // On télécharge la scene pour jouer en arrière plan.
      AsyncOperation sceneArrierePlan = SceneManager.LoadSceneAsync("Main", LoadSceneMode.Additive);

      // On attend que la dernière opération se charge complètement pour retourner quoi que ce soit.
      while (!sceneArrierePlan.isDone)
         yield return null;

      // On déplace l'object "SaveHandler" (vous l'attachez dans l'inspecteur) à la scène nouvellement chargé.
      SceneManager.MoveGameObjectToScene(controllerSauvegarde.gameObject, SceneManager.GetSceneByName("Main"));

      // On décharge la scène du menu.
      SceneManager.UnloadSceneAsync(sceneCourrant);
   }


   public void quitter()
   {
      // On sélectionne la zone de saisis du nom d'utilisateur.
      gestionnaire.SetSelectedGameObject(utilisateurConnexion);

      // Si la connexion n'est pas fermée, on ferme la connexion.
      if (controllerSauvegarde._dbHandler.con != null && controllerSauvegarde._dbHandler.con.State.ToString() != "Closed")
         controllerSauvegarde._dbHandler.con.Close();

      // On actionne le contrôler SQL.
      db.SetActive(true);
                                                               
      // On renvoie l'utilisateur dans le menu de connexion.
      canvasAnimation.SetTrigger("RetourLogin");
   }

   public void suppresion()
   {
      controllerSauvegarde.Delete();
   }
   #endregion

   #region Méthode privé                   
   private void initialisationAttribut()
   {
      // On relie l'animation du canvas avec "canvas".
      canvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On relie le gestionnaire avec le "EventSystem" des menus.
      gestionnaire = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }

   #endregion
}
