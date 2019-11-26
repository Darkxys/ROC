// But : Construire la navigation entre les boutons et les fenêtres des menus.
// Auteur : Gabriel Duquette Godon
// Date : 13 novembre 2019

#region Bibliothèque
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
#endregion

public class Naviguation : MonoBehaviour
{
   // Information pour la recherche pour la construction de la classe.
   // https://docs.unity3d.com/2019.1/Documentation/ScriptReference/EventSystems.EventSystem.html

   #region Attribut
   public EventSystem GestionnaireSysteme { get; private set; }

   public GameObject UtilisateurField { get; private set; }
   #endregion

   private void OnEnable()
   {
      // on prend le "Event System" du canvas.
      GestionnaireSysteme = GameObject.FindGameObjectWithTag("EventSystem").GetComponent<EventSystem>();

      // On intialise l'attribut "UtilisateurField".
      UtilisateurField = GameObject.FindGameObjectWithTag("UtilisateurConnection");

      // On met la zone saisis d'utilisateur active.
      GestionnaireSysteme.firstSelectedGameObject = UtilisateurField;


   }

   private void Update()
   {
      if(Input.GetKeyDown(KeyCode.Tab))
      {
         if(GestionnaireSysteme.currentSelectedGameObject != null && GestionnaireSysteme.currentSelectedGameObject.GetComponent<Selectable>() != null)
         {
            // DÉclaration de la variable local.
            Selectable prochain = GestionnaireSysteme.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            // Si le prochain n'est pas null, fait ceci.
            if(prochain != null)
            {
               InputField zonesaisis = prochain.GetComponent<InputField>();

               if(zonesaisis != null)
               {
                  zonesaisis.OnPointerClick(new PointerEventData(GestionnaireSysteme));
               }

               GestionnaireSysteme.SetSelectedGameObject(prochain.gameObject);
            }
            else
            {
               prochain = Selectable.allSelectablesArray[0];
               GestionnaireSysteme.SetSelectedGameObject(prochain.gameObject);
            }
         }
      }
   }

}
