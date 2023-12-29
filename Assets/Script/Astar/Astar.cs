using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Astar{

    public static Dictionary<Point, node> nodes; /*node has gridposition, tileScript, worldposition and its parent*/

    /*create the direction*/
    private static void CreateNodes()
    { 
        nodes = new Dictionary<Point, node>();

        foreach(TileScript tile in LevelManager.Instance.Tiles.Values){
            nodes.Add(tile.GridPosition, new node(tile));
        }
    }

    /*calculate the final path from start to goal*/
    public static Stack<node> GetPath(Point start, Point goal)
    {
        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<node> openList = new HashSet<node>();

        HashSet<node> closedList = new HashSet<node>();

        Stack<node> finalPath = new Stack<node>();

        node currentNode = nodes[start];

        //1. Add the start node to the open List
        openList.Add(currentNode);


        while (openList.Count > 0)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Point neighbourPos = new Point(currentNode.GridPosition.x - x, currentNode.GridPosition.y - y);

                    if (LevelManager.Instance.InBounds(neighbourPos) && neighbourPos != currentNode.GridPosition && LevelManager.Instance.Tiles[neighbourPos].walkAble)
                    {
                        int gCost = 0;

                        //[14][10][14]
                        //[10][  ][10]
                        //[14][10][14]
                        if (Math.Abs(x - y) == 1)
                        {
                            gCost = 10;
                        }
                        else
                        {
                            if(!ConnectedDiagonnally(currentNode, nodes[neighbourPos])){
                                continue;
                            }
                            //gCost = 14;
                            gCost = 21;
                        }

                        //3. add the neighbor to the open list
                        node neighbour = nodes[neighbourPos];

                        if (openList.Contains(neighbour))
                        {
                            if (currentNode.G + gCost < neighbour.G)
                            {
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);
                            }
                        }
                        else if (!closedList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                            neighbour.CalcValues(currentNode, nodes[goal], gCost);
                        }
                    }
                }
            }

            //5. Moves the current node from the open list to the close tile
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (openList.Count > 0)
            {
                //sort the open list by F value and take the first value from array
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if(currentNode == nodes[goal])
            {
                while(currentNode.GridPosition != start)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                //GameObject.Find("AstarDebugger").GetComponent<AstarDebugger>().DebugPath(openList, closedList, finalPath);
                return finalPath;
            }
        }

        /*This is only for debugging*/
        //GameObject.Find("AstarDebugger").GetComponent<AstarDebugger>().DebugPath(openList, closedList, finalPath);

        return null;
    }

    /*monster cannot cross the disgonnally if there has two towers*/
    public static bool ConnectedDiagonnally(node currentNode, node neighbour)
    {
        Point direction = neighbour.GridPosition - currentNode.GridPosition;

        //Position of two tower in diagonnally
        Point first = new Point(currentNode.GridPosition.x + direction.x, currentNode.GridPosition.y);

        Point second = new Point(currentNode.GridPosition.x, currentNode.GridPosition.y + +direction.y);

        //if two position has tower there then return false
        if(LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].walkAble)
        {
            return false;
        }
        if (LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[first].walkAble)
        {
            return false;
        }

        return true;
    }
}
