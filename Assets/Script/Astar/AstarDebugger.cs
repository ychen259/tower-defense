using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarDebugger : MonoBehaviour {
    [SerializeField]
    private TileScript goal, start;

    [SerializeField]
    private Sprite blankTile;

    [SerializeField]
    private GameObject arrowPrefab;

    public GameObject debugTilePreb;

    //public void Update()
    //{
    //    ClickTile();

    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        Astar.GetPath(start.GridPosition, goal.GridPosition);
    //    }
    //}
    
    /*first right click the tile will be the start point, second right click tile will be end point*/
    private void ClickTile()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null)
            {
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if(tmp != null)
                {
                    if(start == null)
                    {
                        start = tmp;
                        //start.spriteRenderer.color  = Color.green;

                        CreateDebugTile(start.WorldPosition, new Color32(255, 135, 0, 255));
                    }
                    else if(goal == null)
                    {
                        goal = tmp;
                        //goal.spriteRenderer.color = new Color(255, 0, 0, 255);
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));
                    }
                }
            }
        }
    }

    /*show the final path*/
    public void DebugPath(HashSet<node> openList, HashSet<node> closedList, Stack<node> path)
    {
        foreach(node node in openList)
        {
            if (node.TileRef != start && node.TileRef != goal)
            {
                //node.TileRef.spriteRenderer.color = Color.cyan;
                CreateDebugTile(node.TileRef.WorldPosition, Color.black, node);
            }

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach (node node in closedList)
        {
            if (node.TileRef != start && node.TileRef != goal && !path.Contains(node))
            {
                //node.TileRef.spriteRenderer.color = Color.cyan;
                CreateDebugTile(node.TileRef.WorldPosition, Color.blue, node);
            }
        }

        foreach(node node in path){
            if (node.TileRef != start && node.TileRef != goal)
            {
                //node.TileRef.spriteRenderer.color = Color.cyan;
                CreateDebugTile(node.TileRef.WorldPosition, Color.green, node);
            }
        }
    }

    /*arrow point to their parent*/
    private void PointToParent(node node, Vector2 position)
    {
        if(node.Parent != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);

            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;
            /*right*/
            if ((node.GridPosition.x < node.Parent.GridPosition.x) && node.Parent.GridPosition.y == node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            /*Top right*/
            else if ((node.GridPosition.x < node.Parent.GridPosition.x) && node.Parent.GridPosition.y < node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            /*bottom right*/
            else if ((node.GridPosition.x < node.Parent.GridPosition.x) && node.Parent.GridPosition.y > node.GridPosition.y)
            {
                 arrow.transform.eulerAngles = new Vector3(0, 0, -45);
            }
             /*left*/
            else if ((node.GridPosition.x >node.Parent.GridPosition.x) && node.Parent.GridPosition.y == node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            /*Top left*/
            else if ((node.GridPosition.x > node.Parent.GridPosition.x) && node.Parent.GridPosition.y < node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            /*bottom left*/
            else if ((node.GridPosition.x > node.Parent.GridPosition.x) && node.Parent.GridPosition.y > node.GridPosition.y)
            {
                 arrow.transform.eulerAngles = new Vector3(0, 0, -135);
            }
            /*Top*/
            else if ((node.GridPosition.x == node.Parent.GridPosition.x) && node.Parent.GridPosition.y < node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            /*bottom*/
            else if ((node.GridPosition.x == node.Parent.GridPosition.x) && node.Parent.GridPosition.y > node.GridPosition.y)
            {
                arrow.transform.eulerAngles = new Vector3(0, 0, -90);
            }
        }

    }

    /*create debug tile with g,h,f value*/
    public void CreateDebugTile(Vector3 worldPos, Color32 color, node node = null)
    {
        GameObject debugTile = Instantiate(debugTilePreb, worldPos, Quaternion.identity);
        debugTile.GetComponent<RectTransform>().localPosition = worldPos;
       
         if(node != null)
         {
            DebugTile tmp = debugTile.GetComponent<DebugTile>();
            tmp.g.gameObject.SetActive(true);
            tmp.g.text += node.G;

            tmp.h.gameObject.SetActive(true);
            tmp.h.text += node.H;

            tmp.f.gameObject.SetActive(true);
            tmp.f.text += node.F;
        }

        debugTile.GetComponent<SpriteRenderer>().color = color;
    }
}
