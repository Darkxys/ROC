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

   #endregion

   #region Méthode publique
   public void Creation_Compte()
   {
      SceneManager.LoadScene("CreationCompte", LoadSceneMode.Single);
   }
   #endregion

   #region Méthode privé
   public void Fermer_Application()
   {
      Application.Quit();
   }
   #endregion

}