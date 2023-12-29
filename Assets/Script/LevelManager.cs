using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : singleTon<LevelManager> {

    public GameObject [] TileArray; /*tile array from library*/

    public CameraMovement cameraMovement; /*camera movement*/
    public GameObject Map; /*parent of all tiles*/


    public Point blueSpawn, redSpawn; //position of blue and red portal

    public GameObject bluePortalPreb; /*object of blue portal*/
    public GameObject redPortalPreb; /*object of the red portal*/

    public Portal BluePortal; /*Script of the blue portal*/
    public Portal RedPortal; /*Script of the red portal*/

    public Point mapSize; /*size of the map*/

    public Stack<node> Path; /*path from start to destination*/

    /*dictionary is similar with has map*/
    public Dictionary<Point, TileScript> Tiles { set; get;} /*the tilescript and its gridition*/

    /*tile length*/
    public float tileLength
    {
        get { return TileArray[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x; }
    }

    private void Start()
    {
        changeTileSize(); /*modify all tile to the same size*/
        CreateLevel(); /*create the map*/

    }

    /*swap function*/
    public void Swap<T>(ref T a,ref T b)
    {
        T temp = a;
        a = b;
        b = temp;
    }

    /*modify all tile and portal to the same size*/
    public void changeTileSize()
    {
        float size = TileArray[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        float scaleX;
        float scaleY;

        for(int i = 1; i < TileArray.Length; i++)
        {
            scaleX = size / TileArray[i].GetComponent<SpriteRenderer>().sprite.bounds.size.x ;
            scaleY = size / TileArray[i].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
            TileArray[i].GetComponent<Transform>().localScale = new Vector3(scaleX, scaleY, 1);
        }

        scaleX = size / bluePortalPreb.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        scaleY = size / bluePortalPreb.GetComponent<SpriteRenderer>().sprite.bounds.size.y;
        bluePortalPreb.GetComponent<Transform>().localScale = new Vector3(scaleX, scaleY, 1);
        redPortalPreb.GetComponent<Transform>().localScale = new Vector3(scaleX, scaleY, 1);

    }

    /*create the map depend on the mapdata*/
    public void CreateLevel()
    {
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        mapSize = new Point(mapX, mapY);

        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for(int y = 0; y < mapY; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();

            for(int x = 0; x < mapX; x++)
            {
                placeTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

         Vector3 maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;
         
         cameraMovement.SetLimit(new Vector3(maxTile.x + tileLength, maxTile.y - tileLength));

         SpawnPortal();
    }

    /* return the position of last tile*/
    public void placeTile(string tileType, int i, int j, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType); 
    
        TileScript newTile = Instantiate(TileArray[tileIndex]).GetComponent<TileScript>();

        newTile.Setup(new Point(i, j), new Vector3(worldStart.x + tileLength * i, worldStart.y - tileLength * j, 0),
            Map);

    }

    /*read the map data from the level.txt files*/
    public string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);

        return data.Split('-');
    }

    /*create the portal and its position*/
    public void SpawnPortal()
    {
        blueSpawn = new Point(0, 2);
        GameObject tmp = Instantiate(bluePortalPreb, new Vector2(Tiles[blueSpawn].WorldPosition.x, Tiles[blueSpawn].WorldPosition.y+0.15f), Quaternion.identity);
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        redSpawn = new Point(8, 5);
        tmp = Instantiate(redPortalPreb, new Vector2(Tiles[redSpawn].WorldPosition.x, Tiles[redSpawn].WorldPosition.y + 0.15f), Quaternion.identity);
        RedPortal = tmp.GetComponent<Portal>();
        RedPortal.name = "RedPortal";
    }

    /*check if the point is outside of the map or not*/
    public bool InBounds(Point position)
    {
        return position.x >= 0 && position.y >= 0 && position.x < mapSize.x && position.y < mapSize.y; 
    }

    public void GeneratePath()
    {
        Path = Astar.GetPath(blueSpawn, redSpawn);
    }
}