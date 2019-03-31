using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Bird;

    private GameManager manager = null;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Bird.transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager)
            return;

        if (!manager.Dead)
            transform.position = new Vector3(Bird.transform.position.x, transform.position.y, transform.position.z);
    }

    public void RegisterManager(GameManager manager)
    {
        this.manager = manager;
    }
}
