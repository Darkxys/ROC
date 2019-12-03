using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemsHandler : MonoBehaviour
{
    [SerializeField] private Game _game;
    [SerializeField] private GameObject _ItemPrefab;
    [SerializeField] private ShopHandler SH;
    [SerializeField] Text Level;
    [SerializeField] Text Nom;
    [SerializeField] Text Stat;
    [SerializeField] Text Prix;

    public Items selectedItem;

    List<Items> _LstItems;



    private void Awake()
    {

        _LstItems = new List<Items>();
        SaveDB _dbHandler = _game._handler._dbHandler;

        MySqlDataReader reader;
        MySqlCommand command = _dbHandler.con.CreateCommand();

        command.CommandText = "SELECT * FROM items";
        reader = command.ExecuteReader();

        while (reader.Read())
        {
            Items item = new Items(reader);
            _LstItems.Add(item);
        }
        reader.Close();

        UpdateDataList();

    }

    public void UpdateDataList()
    {

        foreach (Items item in _LstItems)
        {
            GameObject go = Instantiate(_ItemPrefab);


            go.transform.parent = this.transform;
            go.transform.localScale = new Vector3(1, 1, 1);


            go.GetComponent<ItemSlot>().item = item;
        }


    }

    public void updateSeletedItem()
    {
        SH.effacerErr();
        Nom.text = selectedItem.Nom.ToString();
        Level.text = selectedItem.Niveau.ToString();
        Prix.text = selectedItem.Prix.ToString() + " $";

        string stats = "";

        if (selectedItem.Attaque > 0)
        {
            stats += "+ " + selectedItem.Attaque.ToString() + "Attaque  \n " ; 
        }
        if (selectedItem.Armure > 0)
        {
            stats += "+ " + selectedItem.Armure.ToString() + "Armure  \n ";
        }
        if (selectedItem.Vie > 0)
        {
            stats += "+ " + selectedItem.Vie.ToString() + "Vie  \n ";
        }



        Stat.text = stats;
        

    }
}
