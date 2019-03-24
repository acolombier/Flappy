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
    private UnityEngine.UI.Text gatesLabel;

    private int maxGatesCache = 5;
    private int precachedGates = 2;

    private void Awake()
    {
        gatesLabel = GateCounter.GetComponent<UnityEngine.UI.Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Gates.Clear();
        SpawnGate();
        SpawnGate();
    }

    private void SpawnGate()
    {
        int offset = Gates.Count < precachedGates ? precachedGates : 1; // If it is the first gate of the game, we put a warmup offset

        for (int i = 0; i < Gates.Count; i++)
        {
            if (!((Gate)Gates[i]).passed)
                offset++;
            else break;
        }

        Gate gate = Instantiate(GatePrefab, Bird.transform.position + new Vector3(offset * GateSpace, 0, 0), Quaternion.identity).GetComponent<Gate>();

        gate.Bird = Bird;

        Gates.Insert(0, gate);

        if (Gates.Count > maxGatesCache)
        {
            Destroy(((Gate)Gates[maxGatesCache]).gameObject);
            Gates.RemoveAt(maxGatesCache);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (((Gate)Gates[1]).passed)
        {
            gatePassed++;
            SpawnGate();
        }
        gatesLabel.text = gatePassed.ToString();

    }
}
