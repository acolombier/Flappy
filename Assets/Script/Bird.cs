using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    public float Speed = 50f;
    public float JumpBoost = 250f;
    public float CoolDownTime = 2;
    public float Gravity = 1f;
    
    private Rigidbody rigitBody;
    private float knockedOutAt;

    // Start is called before the first frame update
    void Awake()
    {
        knockedOutAt = 0;
        rigitBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigitBody.velocity = new Vector3(Speed, 0, 0);
        Physics.gravity = new Vector3(0, -Gravity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (knockedOutAt == 0)
        {
            if (rigitBody.velocity.x != Speed || transform.position.y < -6.5f)
            {
                knockedOutAt = Time.fixedTime;
            }
            else if (Input.GetButtonDown("Jump"))
            {
                rigitBody.velocity += new Vector3(0, JumpBoost, 0);
            }
        }
        else if (knockedOutAt != 0 && knockedOutAt + CoolDownTime < Time.fixedTime)
        {
            SceneManager.LoadScene(0);
        }
    }
}
