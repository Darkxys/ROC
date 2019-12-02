// But : Programmation du menu des héros.
// Auteur : Gabriel Duquette Godon
// Date : 29 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
#endregion

public class MHero : Menu
{
   #region Attribut
   BDHero baseDeDonneHero;
   #endregion

   #region Méthode Unité

   #endregion

   #region Méthode publique
   public void commencerPartie()
   {
      // On démolie pas le menu d'héros.
      DontDestroyOnLoad(this);

      // On envoie l'utilisateur dans la scene principal du jeu.
      SceneManager.LoadScene("Main", LoadSceneMode.Single);
   }

   

   public override void typeMenu()
   {
      Debug.Log("je suis le menu d'héro");
   }
   #endregion

   #region Méthode privé

   #endregion
}
