using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public int size;
    public int[] layerGround;
    public int[] layerCollision;
    public int[] layerAir;

    public Room(int _size, int[] ground, int[] collision = null, int[] air = null)
    {
        size = _size;

        layerGround = ground;

        if (collision == null) layerCollision = InitArray();
        else layerCollision = collision;

        if (air == null) layerAir = InitArray();
        else layerAir = air;
    }

    private int[] InitArray()
    {
        int[] array = new int[size * size];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                array[i*size + j] = 0;
            }
        }

        return array;
    }
}
