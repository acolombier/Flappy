using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public float Speed = 50f;
    public float JumpBoost = 250f;
    public float CoolDownTime = 2;
    
    private Rigidbody rigitBody;
    private float knockedOut = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        rigitBody = GetComponent<Rigidbody>();

    }

    void Start()
    {
        rigitBody.velocity = new Vector3(Speed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (knockedOut == 0)
        {
            if (rigitBody.velocity.x != Speed || transform.position.y < -6.5f)
            {
                knockedOut = Time.fixedTime;

                Debug.Log("KO");
            }
            else if (Input.GetButtonDown("Jump"))
            {
                rigitBody.velocity += new Vector3(0, JumpBoost, 0);
            }
        }
        else if (knockedOut != 0 && knockedOut + CoolDownTime < Time.fixedTime)
        {
            SceneManager.LoadScene(0);
        }
    }
}
