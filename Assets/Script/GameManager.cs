using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float GateSpace = 10f;

    public GameObject GatePrefab;
    public GameObject Bird;
    public GameObject GateCounter;

    private ArrayList Gates = new ArrayList();
    private int gatePassed = 0;

    // Start is called before the first frame update
    void Start()
    {
        SpawnGate();
    }

    private void SpawnGate()
    {
        Gate gate = Instantiate(GatePrefab, Bird.transform.position + new Vector3(GateSpace, 0, 0), Quaternion.identity).GetComponent<Gate>();

        gate.Bird = Bird;

        Gates.Insert(0, gate);

        for (int g = Gates.Count - 1; g > 0; --g)
        {
            gate = (Gate)Gates[g];
            if (!gate.readyToDelete)
                break;
            Destroy(gate.gameObject);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (((Gate)Gates[0]).passed)
        {
            gatePassed++;
            SpawnGate();
        }
        GateCounter.GetComponent<UnityEngine.UI.Text>().text = gatePassed.ToString();

    }
}
