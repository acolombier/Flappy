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

    private void Awake()
    {
        gatesLabel = GateCounter.GetComponent<UnityEngine.UI.Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Gates.Clear();
        SpawnGate();
    }

    private void SpawnGate()
    {
        Gate gate = Instantiate(GatePrefab, Bird.transform.position + new Vector3(GateSpace, 0, 0), Quaternion.identity).GetComponent<Gate>();

        gate.Bird = Bird;

        Gates.Insert(0, gate);

        if (Gates.Count > 3)
        {
            Destroy(((Gate)Gates[3]).gameObject);
            Gates.RemoveAt(3);
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
        gatesLabel.text = gatePassed.ToString();

    }
}
