using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : singleTon<TileScript> {

    public Point GridPosition { get; set; }

    public bool IsEmpty; /*has tower or not*/

    private Color32 fullColor = new Color32(255, 118, 118, 225); //red
    private Color32 emptyColor = new Color32(96, 255, 90, 255); //green

    public SpriteRenderer spriteRenderer; /*spriteRenderer of current tile*/

    private Tower myTower; /*the attack range of the tower*/

    public bool walkAble; /*has tower or not*/

    //public bool Debugging;
    public float monsterSpeed; /*moving speed for monster*/

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        if (LevelManager.Instance.blueSpawn == GridPosition || LevelManager.Instance.redSpawn == GridPosition)
        {
            IsEmpty = false;
        }
    }

    /*center position of the tile*/
    public Vector2 WorldPosition
    {
        get
        {
            return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x / 2),
                transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));
        }
    }


    /*initial the value*/
    /*parent, all tile should put under the Map gameobject*/
    public void Setup(Point gridPos, Vector3 worldPos, GameObject parent)
    {
        walkAble = true;
        IsEmpty = true;

        this.GridPosition = gridPos;
        transform.position = worldPos;

        transform.parent = parent.transform;
        LevelManager.Instance.Tiles.Add(gridPos, this);
    }

    public void OnMouseOver()
    {
        /*if  button and tile stack together, cannot place tile*/
        /*can place the tower only if choose a tower and the tile is not stack with other object*/
        if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
        {

            //Debug.Log("GridPosition    x: " + GridPosition.x + "    y: " + GridPosition.y);
            if (IsEmpty)
            {
                ColorTile(emptyColor); /*color the tile to green*/
                if (Input.GetMouseButtonDown(0))
                {
                    PlaceTower();
                }
            }
            else
            {
                ColorTile(fullColor); /*color the tile to red*/
            }
        }
        /*if nothing stack with the tile and not tower button is choose, you can show the attack range of the tower*/
        else if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            /*there is a tower in current tile*/
            if(myTower != null)
            {
                GameManager.Instance.SelectTower(myTower); /*show the attack range*/
            }
            else
            {
                GameManager.Instance.DeselectTower(); /*如果鼠标在空地点击，取消当前塔的攻击范围*/
            }
        }
    }

    /*put down the tower on the tle*/
    public void PlaceTower() 
    {
        /*make sure we cannot place tower to block the monster*/
        walkAble = false;
        IsEmpty = false;

        /*if monster cannot find a path == monster is blocked by tower*/
        if(Astar.GetPath(LevelManager.Instance.blueSpawn, LevelManager.Instance.redSpawn) == null)
        {
            /*we do not have path*/
            IsEmpty = true;
            walkAble = true;
            return;
        }

        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, WorldPosition, Quaternion.identity);
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.y;

        tower.transform.parent = transform;

        this.myTower = tower.transform.GetChild(0).GetComponent<Tower>();

        myTower.price = GameManager.Instance.ClickedBtn.price;

        //IsEmpty = false;
        //walkAble = false;
        GameManager.Instance.BuyTower(); /*buy the tower*/
    }

    /*change the color of the tile*/
    public void ColorTile(Color32 newColor)
    {
        spriteRenderer.color = newColor;
    }

    /*reset the tile color back to white*/
    public void OnMouseExit()
    {
            ColorTile(Color.white);

    }
}
