// But : Programmer les informations de base de la notification
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
   public Animator CanvasAnimation { get; private set; }

   public EventSystem GestionnnaireEvenement { get; private set; }

   public Color RougeClaire { get; private set; }

   public Color VertClaire { get; private set; }

   public Text Description { get; set; }

   public Button QuitterBtn { get; private set; }

   public GameObject Vignette { get; private set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      initialisationAttribut();
   }
   #endregion

   #region Méthode publique
   public virtual void changementInfo(string option)
   {
      // On enlève toutes les anciennes fonctions du bouton quitter.
      QuitterBtn.onClick.RemoveAllListeners();
   }   
   
   public virtual void typeNotification()
   {
      Debug.Log("Je suis la notification de base");
   }
   #endregion

   #region Méthode Privé
   private void initialisationAttribut()
   {
      // On relie la variable "description" au champs texte "description" de la notification.
      Description = GameObject.FindGameObjectWithTag("NDescription").GetComponent<Text>();

      // On relie la variable "quitterbtn" avec le boutton "quitter" de la notification.
      QuitterBtn = GameObject.FindGameObjectWithTag("NQuitterBtn").GetComponent<Button>();

      // On relie la variable "canvasAnimation" avec le canvas du menu.
      CanvasAnimation = GameObject.FindGameObjectWithTag("LeCanvas").GetComponent<Animator>();

      // On initialise le gestionnaire d'évènement.
      GestionnnaireEvenement = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();

      // On initialise les deux couleurs.
      VertClaire = new Color(62 / 255f, 229 / 255f, 85 / 255f);
      RougeClaire = new Color(255 / 255f, 85 / 255f, 85 / 255f);

      // On initialise la vignette.
      Vignette = GameObject.FindGameObjectWithTag("NVignette");
   }
   #endregion
}
