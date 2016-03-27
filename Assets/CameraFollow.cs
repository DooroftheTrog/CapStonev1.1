using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform person;

    // Use this for initialization
    void Start()
    {
        var x = person.position.x;
        var y = person.position.y;
        transform.position = new Vector3(x, y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        float x = person.position.x;
        float y = person.position.y;
        transform.position = new Vector3(x, y, transform.position.z);
    }
}
