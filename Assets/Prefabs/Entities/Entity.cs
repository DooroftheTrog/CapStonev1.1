using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour {
    //moves is for how many rooms a person can traverse per turn which is based on the entities speed
    //Strength Speed Knowledge and Sanity are the entities stats
    //movement is for how quickly the entity moves
    public int moves, Strength, Speed, Knowledge, Sanity, movement;
    public int STR=0, SPD=0, KNW=0, SNT=0;
    public bool isTurn, hauntStart = false;

    public bool isDead = false, DisplayUI=false;
    public int order;

    // Use this for initialization
    protected void Start () {
        if(order==0)
        {
            isTurn = true;
        }
        else
        {
            isTurn = false;
        }
    }

    // Update is called once per frame
    protected void Update () {
        if (Strength<=0||Speed <= 0||Knowledge <= 0 || Sanity <= 0)
        {
           if(hauntStart==true)
           {
                isDead=true;
            this.switchTurn();
            Destroy(this.gameObject.GetComponent<SpriteRenderer>());
            Destroy(this.gameObject.GetComponent<BoxCollider2D>());

            }
            else
            {
             //Reset Stats to Base Stats need to make copies of stats so that base stats can be recorded
             //TodDo
            }            
        }
        if (isTurn==true)
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                GetComponent<Rigidbody2D>().transform.position += Vector3.up * Time.deltaTime * movement;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                GetComponent<Rigidbody2D>().transform.position += Vector3.left * Time.deltaTime * movement;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                GetComponent<Rigidbody2D>().transform.position += Vector3.down * Time.deltaTime * movement;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                GetComponent<Rigidbody2D>().transform.position += Vector3.right * Time.deltaTime * movement;
            }
        }
	}

    public void switchTurn()
    {
        if(isTurn==true)
        {
            isTurn = false;

        }
        else
        {
            
            isTurn = true;
            moves = Speed*4;

        }
    }

    public Transform getTrans()
    {
        return GetComponent<Transform>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Hit Object");
        if (coll.gameObject.tag == "Door"&&coll.isTrigger)
        {
            this.moves -= 1;
        }
        
        if(coll.gameObject.tag == "Traitor"||coll.gameObject.tag=="Mob"||coll.gameObject.tag=="Player")
        {
            STR = coll.gameObject.GetComponent<Entity>().Strength;
            SPD = coll.gameObject.GetComponent<Entity>().Speed;
            KNW = coll.gameObject.GetComponent<Entity>().Knowledge;
            SNT = coll.gameObject.GetComponent<Entity>().Sanity;
            DisplayUI = true;
        }     
    }

    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Traitor" || coll.gameObject.tag == "Mob" || coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<Entity>().Strength = STR;
        }
            DisplayUI = false;
    }


}
