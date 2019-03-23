using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject Bird;

    public Vector2 GateSize = new Vector2(0, 2);

    public bool passed { get { return Bird.transform.position.x > transform.position.x;  } }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(1, 1 + Random.Range(GateSize.x, GateSize.y), 1);
        transform.position = new Vector3(transform.position.x, Random.Range(-5, 5), 0);
    }
}
