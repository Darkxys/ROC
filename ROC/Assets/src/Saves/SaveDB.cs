using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveDB
{
    public MySqlConnection con;
    public int userID;
    public SaveDB()
    {
        GameObject go = GameObject.FindWithTag("Connector");
        if (go != null)
        {
            con = go.GetComponent<DataBase>().connection;
            con.Open();
            userID = go.GetComponent<DataBase>().userID;

            GameObject.Destroy(go);
        }
        else
        {
            con = new MySqlConnection();
            userID = -1;
        }
    }
} 
