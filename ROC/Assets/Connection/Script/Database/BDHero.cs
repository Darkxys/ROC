// But : Programmation du module de création, suppression, modification des héros.
// Auteur : Gabriel Duquette Godon et Francis
// Date : 29 novembre 2019

#region Bibliothèque
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#endregion

public class BDHero : DataBase
{
   #region Attribut
   public Sauvegarde selectedSave;
   public SaveDB _dbHandler;
   public int levelDungeon = 1;

   [SerializeField] GameObject _btnPrefab;
   [SerializeField] GameObject _infoContainer;
   [SerializeField] GameObject _btnContainer;
   [SerializeField] GameObject _menu;
   [SerializeField] InputField _nameField;

   private List<Sauvegarde> _lstSave;
   private GameObject _infoActive;
   #endregion

   #region Méthode Unité
   private void OnEnable()
   {
      // On trouve le contrôle sql.
      GameObject[] liste = GameObject.FindGameObjectsWithTag("ControllerSql");

      // Si le tableau est plus grand que 1, fait ceci.
      if(liste.Length > 1)
      {
         BDHero bdH = liste[1].GetComponent<BDHero>();

         _dbHandler = bdH._dbHandler;
         _lstSave = bdH._lstSave;

         Destroy(liste[1]);
      }
      else
      {
         // On initialise les attributs
         _dbHandler = new SaveDB();
         _lstSave = new List<Sauvegarde>();

         // On initialise la liste.
         initialisationListe();
      }

      // On met a jour la liste.
      miseAJourListe();
   }
   #endregion

   #region Méthode publique
   public void afficheInfoSauvegarde()
   {
      // Si l’information qui est active a une valeur, on démolit l’info.
      if (_infoActive != null)
         Destroy(_infoActive);

      // On fait apparaitre une cellule d'info.
      _infoActive = Instantiate(_infoContainer);

      // On met l’information du héros dans le conteneur.
      _infoActive.name = "InfoHero";

      // On initialise la position du conteneur.
      _infoActive.transform.position = _menu.transform.position;
      _infoActive.transform.parent = _menu.transform;

      // On initialise le héros avec le héros de la sauvegarde qui est active.
      Hero hero = selectedSave.hero;

      // On crée un tableau de « string » avec les infos du héros.
      string[] info = { hero.nom, hero.niveau.ToString(), hero.exp.ToString(), selectedSave.levelMax.ToString() };

      // On boucle tout le tableau d’information et on insère l’info dans le conteneur.
      for (int compte = 0; compte < info.Length; compte++)
         _infoActive.transform.GetChild(compte).GetComponent<Text>().text += info[compte];

      // On met le niveau max du donjon dans la variable « levelDungeon ».
      levelDungeon = selectedSave.levelMax;
   }

   public void Supprimer()
   {
      // On supprime la sauvegarde.
      suppresionSauvegarde();

      // On supprime le héros.
      suppresionHero();

      // On enlève la sauvegarde dans la liste.
      _lstSave.Remove(selectedSave);

      // On met à jour la liste.
      miseAJourListe();
   }

   public void miseAJourListe()
   {
      // On boucle tous les enfants et leurs conteneurs.
      foreach (Transform enfant in _btnContainer.transform)
         Destroy(enfant.gameObject);

      // On boucle tous la liste de sauvegarde.
      foreach (Sauvegarde lasauvegarde in _lstSave)
      {
         // On fait apparaitre le « prefab » du bouton.
         GameObject partir = Instantiate(_btnPrefab);

         // Si on est à la première sauvegarde, on la sélectionne.
         if (selectedSave == _lstSave[0])
            partir.GetComponent<Button>().Select();

         // On crée la dimension du bouton.
         partir.transform.parent = _btnContainer.transform;

         // On met le nom du héros et son niveau comme description du bouton.
         partir.transform.GetChild(0).GetComponent<Text>().text = "Nom : " + lasauvegarde.hero.nom + " | " + lasauvegarde.hero.niveau;

         // On met la sauvegarde dans le bouton.
         partir.GetComponent<BtnSave>().save = lasauvegarde;
      }

      // Si la liste est plus grande que zéro d’élément, tu fais ceci.
      if (_lstSave.Count > 0)
      {
         // On sélectionne la première sauvegarde.
         selectedSave = _lstSave[0];

         // On affiche les informations de la sauvegarde.
         afficheInfoSauvegarde();
      }
   }


   #endregion

   #region Méthode privé
   private void suppresionSauvegarde()
   {
      // On fait la requête pour la suppresion de la sauvegarde.
      Requete = "DELETE FROM sauvegardes WHERE SauvegardeID = '" + selectedSave.id + "'";

      // On crée une nouvelle commande et on insère la requête dedans.
      Commande = _dbHandler.con.CreateCommand();
      Commande.CommandText = Requete;

      // On exécute la commande.
      Commande.ExecuteNonQuery();
   }

   private void suppresionHero()
   {
      // On fait la requête pour la suppression du héros.
      Requete = "DELETE FROM heros WHERE HerosID = '" + selectedSave.hero.id + "'";

      // On crée une nouvelle commande et on insère la requête dedans.
      Commande = _dbHandler.con.CreateCommand();
      Commande.CommandText = Requete;

      // On exécute la commande de suppression du héros.
      Commande.ExecuteNonQuery();

      // On fait la requête pour supprimer les compétences du héros.
      Requete = "DELETE FROM competencesheros WHERE idHero = '" + selectedSave.id + "'";

      // On crée une nouvelle commande et on insère la requête dedans.
      Commande = _dbHandler.con.CreateCommand();
      Commande.CommandText = Requete;

      // On exécute la commande pour supprimer les compétences du héros.
      Commande.ExecuteNonQuery();

      // On fait la requête pour supprimer tous les objets du héros.
      Requete = "DELETE FROM itemheros WHERE idHero = '" + selectedSave.id + "'";

      // On crée une nouvelle commande et on insère la requête dedans.
      Commande = _dbHandler.con.CreateCommand();
      Commande.CommandText = Requete;

      // On exécute la commande de suppression des objets du héros.
      Commande.ExecuteNonQuery();
   }

   private void initialisationListe()
   {
      // On initialise la liste des sauvegardes.
      initialisationListeSauvegarde();

      // On initialise les héros dans les sauvegardes.
      initialisationHeroDansSauvegarde();
   }

   private void initialisationListeSauvegarde()
   {
      // On initialise la commande.
      Commande = _dbHandler.con.CreateCommand();

      // On sélectionne toutes les sauvegardes de l’utilisateur et on exécute la commande.
      Commande.CommandText = "SELECT * FROM sauvegardes WHERE idUtilisateur = '" + _dbHandler.userID.ToString() + "'";
      Lecteur = Commande.ExecuteReader();

      // On boucle toutes les sauvegardes et on les rajoute dans la liste des sauvegardes.
      while (Lecteur.Read())
      {
         Sauvegarde sauvegarde = new Sauvegarde(Lecteur);
         _lstSave.Add(sauvegarde);
      }

      // On ferme la commande.
      Lecteur.Close();
   }

   private void initialisationHeroDansSauvegarde()
   {
      foreach (Sauvegarde lasauvegarde in _lstSave)
      {
         // On initialise la commande.
         Commande = _dbHandler.con.CreateCommand();

         // On sélectionne le héros du joueur et on exécute la commande.
         Commande.CommandText = "SELECT * FROM heros WHERE HerosID = '" + lasauvegarde.heroID + "'";

         // On exécute la commande.
         Lecteur = Commande.ExecuteReader();

         // On se crée un héros qui est vide.
         Hero leHero = null;

         // On lit toutes les informations du héros et on stocke les informations dans sa variable.
         while (Lecteur.Read())
            leHero = new Hero(Lecteur);

         // On stocke le héros dans la sauvegarde.
         lasauvegarde.hero = leHero;

         // On ferme le lecteur.
         Lecteur.Close();
      }
   }
   #endregion                    
}
