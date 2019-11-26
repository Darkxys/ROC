using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public Vector2Int pos {get; set;}
    public int g {get; set;}
    public int h {get; set;}
    public int f {
        get { return g+h; }
    }
    public Cell parent {get; set;}
    public bool visited {get;set;}

    public Cell(Vector2Int pos, int g, int h, Cell parent){
        this.pos = pos;
        this.g = g;
        this.h = h;
        this.parent = parent;
    }
}
