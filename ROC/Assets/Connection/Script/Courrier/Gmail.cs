// But : Programmer l'envoye des messages pour la boite gmail.
// Auteur : Gabriel Duquette Godon
// Date : 25 novembre 2019

#region Bibliothèque
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;
#endregion

public class Gmail : Courrier
{
   #region Méthode publique
   public override void envoyerMessager(string adresseCourrier)
   {
      // On fait la base de la méthode.
      base.envoyerMessager(adresseCourrier);

      // On déclare le liens vers le fichier de configuration des courriers.
      
   }
   #endregion
}
