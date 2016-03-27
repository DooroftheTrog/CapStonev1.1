using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

    //all possible entities
    public Entity[] possibleEntities = new Entity[2];

    //list of instantiated entities
    public List<Entity> gameEntities = new List<Entity>();
    
    public int numPlayers;

    private GameObject PlayerStats;
    private GameObject EnemyStats,DamageCheck;
    private bool DisplayUI = false;
    private Text str, spd, knw, snt, strE, spdE, knwE, sntE, mvt, dmg;

    //list of all possible rooms
    public Room[] possibleRooms = new Room[2];
    //List of all instantiated Rooms
    public List<Room> Rooms = new List<Room>();

    public CameraFollow gameCam;
    private int turnCounter;
    private bool HauntStart;
	// Use this for initialization
	void Start () {
        numPlayers = PlayerPrefs.GetInt("NumPlayers");
        turnCounter = 0;
        int xTrans = 5;
        HauntStart = false;

        //spawn all of the players
        for(int i =0; i < numPlayers; i++)
        {   
            if(possibleEntities[i]!=null)
            {
                
                gameEntities.Add(Instantiate(possibleEntities[i], new Vector2(xTrans, 5), Quaternion.identity) as Entity);
                gameEntities[i].order=i;
                xTrans += 1;
            }            
        }

        PlayerStats = GameObject.Find("PlayerStats");
        EnemyStats = GameObject.Find("EnemyStats");
        DamageCheck = GameObject.Find("Damage");

        str = GameObject.Find("CurStr").GetComponent<Text>();
        spd = GameObject.Find("CurSpeed").GetComponent<Text>();
        knw = GameObject.Find("CurKnow").GetComponent<Text>();
        snt = GameObject.Find("CurSan").GetComponent<Text>();

        mvt = GameObject.Find("CurMoves").GetComponent<Text>();

        strE = GameObject.Find("StrengthCur").GetComponent<Text>();
        spdE = GameObject.Find("SpeedCur").GetComponent<Text>();
        knwE = GameObject.Find("KnowledgeCur").GetComponent<Text>();
        sntE = GameObject.Find("SanityCur").GetComponent<Text>();
        dmg = DamageCheck.GetComponent<Text>();

        PlayerStats.SetActive(true);
        EnemyStats.SetActive(false);
        DamageCheck.SetActive(false);

        gameCam.person = gameEntities[turnCounter].transform;
        createRoom(new Vector2(0, 0), -1);
        gameEntities[turnCounter].switchTurn();
    }

    // Update is called once per frame
    void Update () {

        if (gameEntities[turnCounter].isDead == true)
        {
            //if the entity is dead then remove it from the board 
            Destroy(gameEntities[turnCounter]);
            gameEntities.RemoveAt(gameEntities.Count-1);
            if(turnCounter < gameEntities.Count - 1||turnCounter>gameEntities.Count-1)
            {
                turnCounter = 0;
            }
            gameCam.person = gameEntities[turnCounter].transform;
            gameEntities[turnCounter].switchTurn();
        }

        if (gameEntities[turnCounter].moves==0)
        {
            gameEntities[turnCounter].switchTurn();
            Debug.Log(gameEntities.Count);
            if(turnCounter<gameEntities.Count-1)
            {
                turnCounter++;
            }
            else
            {
                turnCounter = 0;
            }
            gameEntities[turnCounter].switchTurn();
            gameCam.person = gameEntities[turnCounter].transform;
         }

        if(gameEntities[turnCounter].DisplayUI==true)
        {
            strE.text = "Strength: " + gameEntities[turnCounter].STR;
            spdE.text = "Speed: " + gameEntities[turnCounter].SPD;
            knwE.text = "Knowledge: " + gameEntities[turnCounter].KNW;
            sntE.text = "Sanity: " + gameEntities[turnCounter].SNT;

            EnemyStats.SetActive(true);
        } else
        {
            DamageCheck.SetActive(false);
            EnemyStats.SetActive(false);
        }

        str.text = "Strength: " + gameEntities[turnCounter].Strength;
        spd.text = "Speed: " + gameEntities[turnCounter].Speed;
        knw.text = "Knowledge: " + gameEntities[turnCounter].Knowledge;
        snt.text = "Sanity: " + gameEntities[turnCounter].Sanity;

        mvt.text = "Moves: " + gameEntities[turnCounter].moves/4;

        //if collision with a unopened door then spawn room	
        checkDoors();
	}

    void checkDoors()
    {
        for (int i = 0; i < Rooms.Count; i++)
        {
            List<Door> toCheck = Rooms[i].getDoors();
            foreach (Door check in toCheck)
                //if a door is open and a new room has not been spawned then create a new room
                if (check.isOpen == true && check.isLoaded == false)
                {
                    
                    Vector2 newPos = check.pos;
                    switch (check.side)
                    {
                        case 0:
                            newPos.y -= 15;
                            //gameEntities[turnCounter].GetComponent<Transform>().position.y -= 6;
                            break;
                        case 1:
                            newPos.x -= 15;
                            //gameEntities[turnCounter].transform.x -= 6;
                            break;
                        case 2:
                            newPos.x += 15;
                            //gameEntities[turnCounter].transform.x += 6;
                            break;
                        case 3:
                            newPos.y += 15;
                            break;
                    }
                    check.isLoaded = true;
                    createRoom(newPos, check.side);
                    gameEntities[turnCounter].moves = 0;
                }
            }
        }

    //loads a new random room
    void createRoom(Vector2 pos, int prev)
    {
        int roomNum = Random.Range(0, possibleRooms.Length);        
            
        //prev.side is used to open the door that is spawned
        var loadRoom = possibleRooms[roomNum];
        loadRoom.prevDoor = prev;
        loadRoom.pos = pos;

        Rooms.Add(Instantiate(loadRoom,pos, Quaternion.identity) as Room);        
    }

    public void GameOver()
    {
        bool gameover = true;
        foreach (Entity check in gameEntities)
        {
            if(check.tag=="Player")
            {
                if(check.isDead!=true)
                {
                    gameover = false;
                }
            }

        }

        if(gameover==true)
        {
            //move to gameover scene
        }
    }

    public void Attack()
    {
        DamageCheck.SetActive(true);
        int PlayerHit = 0;
        int EnemyHit = 0;
        for(int i = 0; i < gameEntities[turnCounter].Strength; i++)
        {
            PlayerHit += Random.Range(0, 2);
        }

        for (int i = 0; i < gameEntities[turnCounter].STR; i++)
        {
            EnemyHit += Random.Range(0, 2);
        }

        int Damage = PlayerHit - EnemyHit;
        if(Damage<0)
        {
            gameEntities[turnCounter].Strength -= Damage;
        }
        else
        {
            gameEntities[turnCounter].STR -= Damage;
        }

        dmg.text = " Damage: " + Damage;
        gameEntities[turnCounter].moves = 0;
    }
}
