// But : Programmer les couleurs des vignettes.
// Auteur : Gabriel Duquette Godon
// Date : 4 décembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class VignetteCouleur : MonoBehaviour
{
   #region Attribut
   private Color RougeClaire { get; set; }

   private Color VertClaire { get; set; }
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // on initialise la couleur vert claire
      VertClaire = new Color(62 / 255f, 229 / 255f, 85 / 255f);
      
      // On initialise la couleur rouge claire.
      RougeClaire = new Color(255 / 255f, 85 / 255f, 85 / 255f);
   }
   #endregion

   #region Méthode publique
   public Color rougeClaire()
   {
      return RougeClaire;
   }

   public Color vertClaire()
   {
      return VertClaire;
   }
   #endregion
}
