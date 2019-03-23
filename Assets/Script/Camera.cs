using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Bird;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Bird.transform.position.x, Bird.transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Bird.transform.position.x, transform.position.y, transform.position.z);   
    }
}
