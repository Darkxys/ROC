// But : Programmer les changements de la fenêtre de notification.
// Auteur : Gabriel Duquette Godon
// Date : 2 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

public abstract class Notification : MonoBehaviour
{
   #region Attribut
   public Animator canvas { get; private set; }   

   public VignetteCouleur couleurVignette { get; private set; }
   
   public Text definition{ get; private set; }

   public EventSystem gestionnaireEvenement { get; private set; }

   public Button quitter { get; private set; }

   public GameObject vignette { get; set; }

   public GameObject utilisateurConnexion;

   public Button JouerBtn;
   public Button RevenirBtn;
   public DataBase data;

   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On initialise les attributs
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique

   /// <summary>
   /// Change les informations de la fenêtre de notification.
   /// </summary>
   /// <param name="option">L’information voulue.</param>
   public virtual void chargementinfoNotification(string option)
   {
      // On enlève toutes les anciennes fonctions du bouton « quitter ».
      quitter.onClick.RemoveAllListeners();

      // On enlève les fonctions des boutons de jouer et revenir.
      JouerBtn.onClick.RemoveAllListeners();
      RevenirBtn.onClick.RemoveAllListeners();

      // On disparait les deux boutons d'action.
      JouerBtn.gameObject.SetActive(false);
      RevenirBtn.gameObject.SetActive(false);
   }
   #endregion

   #region Méthode protégée
   /// <summary>
   /// Envoie l’utilisateur vers la fenêtre de connexion par le bouton « quitter ».
   /// </summary>
   protected virtual void retourConnection()
   {
      gestionnaireEvenement.SetSelectedGameObject(utilisateurConnexion);

      canvas.SetTrigger("RetourLogin");
   }
   #endregion

   #region Méthode privé
   private void initialisationAttribut()
   {
      // On relie la variable "description" au champs texte "description" de la notification.
      definition = GameObject.FindGameObjectWithTag("NDescription").GetComponent<Text>();

      // On relie la variable "quitterbtn" avec le boutton "quitter" de la notification.
      quitter = GameObject.FindGameObjectWithTag("NQuitter").GetComponent<Button>();

      // On relie l'animation du canvas avec "canvas".
      canvas = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On relie la variable "vignette" a l'entête de la notification.
      vignette = GameObject.FindGameObjectWithTag("NVignette");

      // On initialise la variable "couleurVignette".
      couleurVignette = GameObject.FindGameObjectWithTag("CVignette").GetComponent<VignetteCouleur>();

      // On initialise le gestionnaire d'évènement.
      gestionnaireEvenement = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();
   }
   #endregion
}
