// But : Programmer la génération de courrier personnalisé
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

public abstract class Courrier : MonoBehaviour
{
   #region Référence
   // https://answers.unity.com/questions/433283/how-to-send-email-with-c.html
   // https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.mailmessage?view=netframework-4.8
   #endregion

   #region Attribut
   public MailMessage NouveauMessage { get; private set; }

   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      // On initialise le nouveau message.
      NouveauMessage = new MailMessage();
   }
   #endregion

   #region Méthode publique
   public virtual void envoyerMessager(string adresseCourrier)
   {
      // On crée le message.
      creationMessage(adresseCourrier);

      
   }
   #endregion

   #region Méthode privé
   private void creationMessage(string adresseCourrier)
   {
      // On met l'adresse email dans le nouveau message.
      NouveauMessage.From = new MailAddress(adresseCourrier);

      // on rajoue dans la cellule "à".
      NouveauMessage.To.Add(adresseCourrier);

      // On écrit le sujet de l'adresse courrier.
      NouveauMessage.Subject = "Confirmation du compte d'utilisateur";

      // On écrit le corps du message du courrier.
      NouveauMessage.Body = "Félisation, vous avez créez votre utilisateur dans la console.";
   }
   #endregion
}
