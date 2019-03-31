using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float GateSpace = 10f;
    public float CoolDownTime = 2;

    [Header("Object mapping")]
    public GameObject GatePrefab;
    public GameObject BackgroundPrefab;
    public GameObject Bird;
    public GameObject GateCounter;
    public GameObject Camera;

    [Header("Menu mapping")]
    public GameObject Title;
    public Button ActionButton;

    [Header("UI Settings")]
    public float TitleInPosition = 420;
    public float TitleOutPosition = 840;
    public float ButtonInPosition = -600;
    public float ButtonOutPosition = -1200;
    public float MenuJumpThreshold = 0;
    public float TransitTime = 3;

    [Header("Game Settings")]
    public float BackgroundWidth = 32f;
    public float BackgroundLoadedOffset = 64f;

    private List<GameObject> Gates = new List<GameObject>();
    private Queue<GameObject> Backgrounds = new Queue<GameObject>();

    private GameObject lastBackground; // Because CSharp is the worse language ever, no Queue.last()

    private int gatePassed = 0;
    private TextMeshProUGUI gatesLabel;
    private Bird player;
    private int maxGatesCache = 5;
    private int precachedGates = 2;
    
    private float knockedOutAt;
    private float uiProcessTime;
    private float titleDirection;
    private float buttonDirection;

    // Game State
    public bool Running { get { return knockedOutAt == 0f; } }
    public bool Dead { get { return knockedOutAt > 0f; } }

    private void Awake()
    {
        gatesLabel = GateCounter.GetComponent<TextMeshProUGUI>();
        player = Bird.GetComponent<Bird>();

        ActionButton.onClick.AddListener(LaunchGame);

        Camera.GetComponent<Camera>().RegisterManager(this);
        player.RegisterManager(this);
    }

    public void LaunchGame()
    {
        if (Dead || Running)
            return;

        hideUI();

        knockedOutAt = 0;
        player.Jump();
    }

    public bool movingUI { get { return uiProcessTime > 0; } }

    private void proceedUI()
    {
        float percentage = (Time.fixedTime - uiProcessTime) / TransitTime;

        RectTransform canvasRect = Title.GetComponent<RectTransform>();

        // Set our position as a fraction of the distance between the markers.
        canvasRect.localPosition = new Vector3(canvasRect.localPosition.x, Mathf.Lerp(canvasRect.localPosition.y, titleDirection, percentage), 0);

        canvasRect = ActionButton.GetComponent<RectTransform>();

        // Set our position as a fraction of the distance between the markers.
        canvasRect.localPosition = new Vector3(canvasRect.localPosition.x, Mathf.Lerp(canvasRect.localPosition.y, buttonDirection, percentage), 0);

        if (percentage >= 1)
        {
            uiProcessTime = -1;
        }
    }

    private void hideUI()
    {
        uiProcessTime = Time.fixedTime;
        titleDirection = TitleOutPosition;
        buttonDirection = ButtonOutPosition;
    }

    private void showUI()
    {
        uiProcessTime = Time.fixedTime;
        titleDirection = TitleInPosition;
        buttonDirection = ButtonInPosition;
    }

    public void GameOver()
    {
        knockedOutAt = Time.fixedTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        knockedOutAt = -1f;
        gatesLabel.text = "";
        uiProcessTime = -1;

        Gates.Clear();
        Backgrounds.Clear();
        lastBackground = null;

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
        Vector3 gatePosition = Gates.Count == 0 ? 
            Bird.transform.position + new Vector3(precachedGates * GateSpace, 0, 0) :
            Gates[0].transform.position + new Vector3(GateSpace, 0, 0);

        GameObject gate = Instantiate(GatePrefab, gatePosition, Quaternion.identity);

        Gates.Insert(0, gate);

        if (Gates.Count > maxGatesCache)
        {
            Destroy(Gates[maxGatesCache]);
            Gates.RemoveAt(maxGatesCache);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        // UI
        if (movingUI)
            proceedUI();

        // Gate passing
        if (Running)
        {
            if (Gates.Count > 1) {
                if (Gates[1].transform.position.x < player.transform.position.x)
                {
                    gatePassed++;
                    SpawnGate();
                }
            }
            else
            {
                SpawnGate();
            }
            gatesLabel.text = gatePassed.ToString();
        }

        // Background managment
        if (ShouldLoadBackground())
        {
            SpawnBackground();
        }
        if (Bird.transform.position.x > Backgrounds.Peek().transform.position.x + BackgroundLoadedOffset)
        {
            DespawnBackground();
        }

        if (Dead && knockedOutAt + CoolDownTime < Time.fixedTime)
        {
            SceneManager.LoadScene(0);
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
