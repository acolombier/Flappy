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

    public float rotateFactor = 5f;
    public float maxRotate = 45f;
    
    private Rigidbody rigitBody;
    private float knockedOutAt;

    public bool Alive { get { return knockedOutAt == 0f; } }

    // Start is called before the first frame update
    void Awake()
    {
        knockedOutAt = 0;
        rigitBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        rigitBody.velocity = new Vector3(Speed, JumpBoost, 0);
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
            else
            {
#if UNITY_ANDROID
                if (Input.touchCount != 0)
#elif UNITY_STANDALONE || UNITY_WEBPLAYER
                if (Input.GetButtonDown("Jump"))
#endif
                {
                    rigitBody.velocity = new Vector3(Speed, JumpBoost, 0);
                }
                //transform.rotation = new Quaternion(0, 0, 0.5f, 1);
                transform.rotation = new Quaternion(0, 0, Mathf.Clamp(rigitBody.velocity.y / rotateFactor, -1, 1) * maxRotate, 1);
            }
        }
        else if (knockedOutAt != 0 && knockedOutAt + CoolDownTime < Time.fixedTime)
        {
            SceneManager.LoadScene(0);
        }        

    }
}
