// But : Programmer la connection à la base de données de l'application
// Auteur : Gabriel Duquette Godon
// Date : 27 octobre 2019

#region Bibliothèque
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MySql.Data.MySqlClient;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
#endregion

public class DataBase : MonoBehaviour
{
   #region Attribut
   private string Host { get; set; }

   private string BD { get; set; }

   private string Utilisateur { get; set; }

   private string MotDePasse { get; set; }

   protected MySqlConnection Connecteur { get; private set; }

   protected MySqlDataReader Lecteur { get; set; }

   protected MySqlCommand Commande { get; set; }

   protected string Requete { get; set; }

   #endregion

   #region Constante
   const string NOM_FICHIER_CONFIG = "Config.txt";
   #endregion

   #region Méthode Unité
   private void Awake()
   {
      // On trouve le contrôle sql.
      GameObject[] liste = GameObject.FindGameObjectsWithTag("ControllerSql");

      // Si le tableau est plus grand que 1, fait ceci.
      if (liste.Length > 1)
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
   private void OnEnable()
   {
      // Si le fichier existe on le lis, sinon il faut le créee.
      if (!File.Exists(Application.dataPath + "/../" + NOM_FICHIER_CONFIG))
      {
         Debug.Log("Creation du fichier " + NOM_FICHIER_CONFIG + ",");

         CreeFichierConfig();
      }
      else
         Debug.Log("Le fichier " + NOM_FICHIER_CONFIG + " existe !");

      // On lis les informations du fichier de configuration.
      LireFichier();
   }
   #endregion

   #region Méthode protégé
   protected void ConnectionBd()
   {
      // Déclaration de la variable locale.
      string constr = "server=" + Host + ";uid=" + Utilisateur + ";pwd=" + MotDePasse + ";database=" + BD + ";";

      try
      {
         Connecteur = new MySqlConnection(constr);

         // On ouvre la connection.
         Connecteur.Open();
      }
      catch (IOException Ex)
      {
         Debug.LogError(Ex);
      }
   }
   #endregion

   #region Méthode privé

   private void CreeFichierConfig()
   {
      Debug.Log("Création du fichier " + NOM_FICHIER_CONFIG + ".");

      // On crée le fichier
      StreamWriter fichier = new StreamWriter(Application.dataPath + "/../" + NOM_FICHIER_CONFIG);

      // On écrit les informations par défaut du fichier.
      fichier.Write("host=420.cstj.qc.ca\r");
      fichier.Write("database=rock_test\r");
      fichier.Write("username=ROCK\r");
      fichier.Write("password=Rock88811\r");

      // On ferme le fichier.
      fichier.Close();
   }

   private void LireFichier()
   {
      try
      {
         // On ouvre le fichier de config.
         using (StreamReader streamReader = new StreamReader(Application.dataPath + "/../" + NOM_FICHIER_CONFIG))
         {
            // On prend le contenu.
            string contenu = streamReader.ReadToEnd();

            // On crée une liste avec les lignes du documents.
            List<string> lignes = new List<string>(contenu.Split("\r"[0]));

            for (int index = 0; index < lignes.Count(); index++)
            {
               // On trouve la position du égale.
               int position = lignes[index].IndexOf("=");

               switch (index)
               {
                  // On extrait l'information du host
                  case 0:
                     Host = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom de table de la base de données.
                  case 1:
                     BD = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du nom d'utilisateur de la base de données.
                  case 2:
                     Utilisateur = lignes[index].Substring(position + 1);
                     break;

                  // On extrait l'information du mot de passe de la base de données.
                  case 3:
                     MotDePasse = lignes[index].Substring(position + 1);
                     break;
               }
            }
         }
      }
      catch (IOException e)
      {
         Debug.LogError("Le fichier ne peut pas ouvrir.");
         Debug.LogError(e.Message);
      }
   }

   #endregion

   #region Attribut Francis
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

   #region Méthode Francis

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

   public void Return()
   {
      SceneManager.LoadScene("MenuPrincipal", LoadSceneMode.Single);
   }

   public void Create()
   {
      Hero newHero = CreateHero(_nameField.text);
      Sauvegarde newSave = CreateSauvegarde(newHero);
      newSave.hero = newHero;

      _lstSave.Add(newSave);

      miseAJourListe();
   }

   private Sauvegarde CreateSauvegarde(Hero hero)
   {
      string requete =
         "INSERT INTO sauvegardes " +
         "VALUES (default," + _dbHandler.userID + ",default," + hero.id + ")";

      MySqlCommand cmd = _dbHandler.con.CreateCommand();
      MySqlDataReader reader;
      cmd.CommandText = requete;

      cmd.ExecuteNonQuery();
      long idSauvegarde = cmd.LastInsertedId;

      requete = "SELECT * FROM sauvegardes WHERE SauvegardeID = '" + idSauvegarde + "'";

      cmd = _dbHandler.con.CreateCommand();
      cmd.CommandText = requete;

      reader = cmd.ExecuteReader();
      Sauvegarde newSauvegarde = null;
      while (reader.Read())
         newSauvegarde = new Sauvegarde(reader);
      reader.Close();

      return newSauvegarde;
   }

   private Hero CreateHero(string name)
   {
      string requete =
          "INSERT INTO heros " +
          "VALUES (default,'" + name + "',default,default,default,default,default,default)";

      MySqlCommand cmd = _dbHandler.con.CreateCommand();
      MySqlDataReader reader;
      cmd.CommandText = requete;

      cmd.ExecuteNonQuery();
      long idHero = cmd.LastInsertedId;

      requete = "SELECT * FROM heros WHERE HerosID = '" + idHero + "'";

      cmd = _dbHandler.con.CreateCommand();
      cmd.CommandText = requete;

      reader = cmd.ExecuteReader();
      Hero newHero = null;
      while (reader.Read())
         newHero = new Hero(reader);
      reader.Close();

      return newHero;
   }

   public void SaveGame()
   {
      GameObject go = GameObject.FindGameObjectWithTag("player");
      if (!go) return;

      MySqlConnection con = _dbHandler.con;
      MySqlDataReader reader;
      Player player = go.GetComponent<Player>();
      PlayerStats statsPlayer = player.Joueur;
      Stat stat = player.JoueurStats;

      int exp = statsPlayer.Xp;
      int attaque = 10 * statsPlayer.Level;
      int level = statsPlayer.Level;
      int gold = statsPlayer.Gold;
      int vie = (int)stat.MaxValVie;
      List<int> lstItem = statsPlayer.ItemsPos;

      // Update on Hero table
      string request =
          "UPDATE heros " +
          "SET Niveau = " + level + ", Exp = " + exp + ", gold = " + gold + ", vie = " + vie + ", attaque = " + attaque + " " +
          "WHERE HerosID = " + selectedSave.heroID;

      MySqlCommand cmd = con.CreateCommand();
      cmd.CommandText = request;

      cmd.ExecuteNonQuery();

      // Update on all itemshero table
      request =
          "SELECT * FROM itemheros WHERE idHero = " + selectedSave.heroID;

      cmd = con.CreateCommand();
      cmd.CommandText = request;

      reader = cmd.ExecuteReader();
      while (reader.Read())
      {
         foreach (int id in statsPlayer.ItemsPos)
         {
            if (id == Int32.Parse(reader["idHero"].ToString()))
            {
               lstItem.Remove(id);
               break;
            }
         }
      }
      reader.Close();

      foreach (int id in lstItem)
      {
         request =
         "INSERT INTO itemheros" +
         "VALUES (" + id + ", " + selectedSave.heroID + ")";

         cmd = con.CreateCommand();
         cmd.CommandText = request;

         cmd.ExecuteNonQuery();
      }
   }

   #endregion
}
