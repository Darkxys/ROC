using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Salle
{
    public int id;
    public int[] layerFloor;
    public int[] layerCollision;
    public int[] layerAir;
    public int size;

    public Salle(int _id,int[] floor,int[] collision,int[] air,int _size)
    {
        id = _id;
        layerFloor = floor;
        size = _size;

        if (air == null)
            layerAir = ZeroArray();
        else
            layerAir = air;

        if (collision == null)
            layerCollision = ZeroArray();
        else
            layerCollision = collision;

        InitRoom();
    }

    public Salle(MySqlDataReader reader) : this(Int32.Parse(reader["idSalle"].ToString()),JsonConvert.DeserializeObject<int[]>(reader["layerFloor"].ToString()),
        JsonConvert.DeserializeObject<int[]>(reader["layerCollision"].ToString()), JsonConvert.DeserializeObject<int[]>(reader["layerAir"].ToString()), Int32.Parse(reader["size"].ToString())) { }

    private int[] ZeroArray()
    {
        int[] arr = new int[size * size];
        for (int i = 0; i < size*size; i++)
        {
            arr[i] = 0;
        }
        return arr;
    }

    private void InitRoom()
    {
        int[] floor = new int[size * size];
        int[] coll = new int[size * size];
        int[] air = new int[size * size];

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                floor[i * size + j] = layerFloor[(size - i - 1) * size + j];
                coll[i * size + j] = layerCollision[(size - i - 1) * size + j];
                air[i * size + j] = layerAir[(size - i - 1) * size + j];
            }
        }

        layerFloor = floor;
        layerCollision = coll;
        layerAir = air;
    }
}
