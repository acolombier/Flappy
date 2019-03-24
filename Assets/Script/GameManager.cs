using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float GateSpace = 10f;

    public GameObject GatePrefab;
    public GameObject BackgroundPrefab;
    public GameObject Bird;
    public GameObject GateCounter;

    public float BackgroundWidth = 32f;
    public float BackgroundLoadedOffset = 64f;

    private List<Gate> Gates = new List<Gate>();
    private Queue<GameObject> Backgrounds = new Queue<GameObject>();

    private GameObject lastBackground = null; // Because CSharp is the worse language ever, no Queue.last()

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
        Backgrounds.Clear();
        
        SpawnGate();
        SpawnGate();

        while (ShouldLoadBackground())
        {
            SpawnBackground();
        }
    }

    private bool ShouldLoadBackground()
    {
        return !lastBackground || Bird.transform.position.x > lastBackground.transform.position.x - BackgroundLoadedOffset;
    }

    private void SpawnGate()
    {
        int offset = Gates.Count < precachedGates ? precachedGates : 1; // If it is the first gate of the game, we put a warmup offset

        for (int i = 0; i < Gates.Count; i++)
        {
            if (!Gates[i].passed)
                offset++;
            else break;
        }

        Gate gate = Instantiate(GatePrefab, Bird.transform.position + new Vector3(offset * GateSpace, 0, 0), Quaternion.identity).GetComponent<Gate>();

        gate.Bird = Bird;

        Gates.Insert(0, gate);

        if (Gates.Count > maxGatesCache)
        {
            Destroy(Gates[maxGatesCache].gameObject);
            Gates.RemoveAt(maxGatesCache);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        if (Gates[1].passed && Bird.GetComponent<Bird>().Alive) // Might be faster to cache the component
        {
            gatePassed++;
            SpawnGate();
        }
        gatesLabel.text = gatePassed.ToString();

        if (ShouldLoadBackground())
        {
            SpawnBackground();
        }
        if (Bird.transform.position.x > Backgrounds.Peek().transform.position.x + BackgroundLoadedOffset)
        {
            DespawnBackground();
        }

    }

    private void SpawnBackground()
    {
        float offset = lastBackground == null ? -BackgroundLoadedOffset : lastBackground.transform.position.x + BackgroundWidth;

        lastBackground = Instantiate(BackgroundPrefab, new Vector3(offset, 0, 0), Quaternion.identity);
        Backgrounds.Enqueue(lastBackground);
    }

    private void DespawnBackground()
    {
        Destroy(Backgrounds.Dequeue());
    }
}
