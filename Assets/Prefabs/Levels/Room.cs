using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Room : MonoBehaviour {
    private int levelWidth, levelHeight;

    public Transform floorTile, wallTile;

    public Door doorTile;

    public List<Transform> tiles = new List<Transform>();

    public Color floorColor, wallColor, doorColor, spawnpoint;

    public Color[] tileColors;

    public int prevDoor;

    public Texture2D levelTexture;

    public Entity[] entities;

    public Vector2 pos;

    //used to determine what type of event will happen when the room is spawned
    public int eventType;

    public List<Door> DoorList = new List<Door>();
    // Use this for initialization
    void Start () {
        eventType = 0;
        levelWidth = levelTexture.width;
        levelHeight = levelTexture.height;
        loadLevel(prevDoor);
    }

    void Update()
    {

    }

    public void loadLevel(int prevDoor)
    {

        tileColors = new Color[levelWidth * levelHeight];
        tileColors = levelTexture.GetPixels();
        int doorCount = 0;
        int posY = (int)GetComponent<Transform>().position.y;
        int posX = (int)GetComponent<Transform>().position.x;

        //switch the door numer to the opposite door so that the new door does not load the previous room
        switch (prevDoor)
        {
            case 0:
                prevDoor = 3;
                break;
            case 1:
                prevDoor = 2;
                break;
            case 2:
                prevDoor = 1;
                break;
            case 3:
                prevDoor = 0;
                break;
        }

        for (int y = 0; y < levelHeight; y++)
        {
            for (int x = 0; x < levelWidth; x++)
            {
                if (tileColors[x + y * levelWidth] == floorColor)
                {
                    tiles.Add(Instantiate(floorTile, new Vector2(posX, posY), Quaternion.identity)as Transform);
                }
                if (tileColors[x + y * levelHeight] == wallColor)
                {
                    tiles.Add(Instantiate(wallTile, new Vector2(posX, posY), Quaternion.identity)as Transform);
                }
                if (tileColors[x + y * levelHeight] == doorColor)
                {
                   
                    //load the door and then change the variables so that the original is unchanged
                    var loadDoor = Instantiate(doorTile, new Vector2(posX, posY), Quaternion.identity) as Door;
                    loadDoor.side = doorCount;
                    loadDoor.pos = this.pos;
                    if (doorCount == prevDoor)
                    {                      
                        loadDoor.isOpen = true;
                        loadDoor.isLoaded = true;
                    }
                    DoorList.Add(loadDoor);
                    doorCount++;
                }
                if (tileColors[x + y * levelHeight] == spawnpoint)
                {
                    Instantiate(floorTile, new Vector2(posX, posY), Quaternion.identity);

                    //sets single player to the spawn point need to change to make more adaptive in the future
                    if (entities.Length > 0)
                    {
                        entities[0].transform.position = (new Vector2(posX, posY));
                    }
                }
                posX++;
            }
            posX = (int)GetComponent<Transform>().position.x; ;
            posY++;
        }
    }
    public List<Door> getDoors()
    {
        return DoorList;
    }
}
