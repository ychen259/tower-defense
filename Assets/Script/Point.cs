using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Point
{
    public int x { set; get; }
    public int y { set; get; }

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /*define the == operator*/
    public static bool operator ==(Point first, Point second)
    {
        if (first.x == second.x && first.y == second.y) return true;

        return false;
    }

    /*define the != operator*/
    public static bool operator !=(Point first, Point second)
    {
        if (first.x == second.x && first.y == second.y) return false;

        return true;
    }

    /*define the - operator*/
    public static Point operator-(Point first, Point second){
       return new Point(first.x - second.x, first.x -second.y);
    }
}
