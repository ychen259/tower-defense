using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class node{

    public Point GridPosition;

    public TileScript TileRef;

    public Vector2 worldPosition;

    public node Parent;

    public int G;
    public int H;
    public int F;

    public node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.worldPosition = tileRef.WorldPosition;
    }

    /*calculate the h,g,f value*/
    public void CalcValues(node parent, node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = (Math.Abs(GridPosition.x - goal.GridPosition.x) + Math.Abs(goal.GridPosition.y - GridPosition.y)) * 10;
        this.F = G + H;
    }
}
