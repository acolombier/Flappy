using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject Bird;

    public Vector2 GateSize = new Vector2(0, 1);
    public Vector2 GatePosition = new Vector2(-3, 4);

    public bool passed { get { return Bird.transform.position.x > transform.position.x;  } }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 1 + Random.Range(GateSize.x, GateSize.y), 1);
        transform.position = new Vector3(transform.position.x, Random.Range(GatePosition.x, GatePosition.y), 0);
    }
}
