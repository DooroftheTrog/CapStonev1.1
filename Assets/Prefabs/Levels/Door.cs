using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {
    public Vector2 pos = new Vector2();
    public int side=-1;
    public bool isOpen = false, isLoaded = false;
	// Use this for initialization
	void Start ()
    {

	}

    void OnTriggerEnter2D(Collider2D coll)
    {
            if (coll.gameObject.tag == "Player" && isOpen == false)
            {
                Debug.Log("door is open");
                isOpen = true;
            }
        
    }
}
