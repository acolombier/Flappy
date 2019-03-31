using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float Speed = 50f;
    public float JumpBoost = 250f;
    public float Gravity = 1f;

    [Header("Animation binding")]
    public string FlyAnimationName = "Fly";

    public float rotateFactor = 5f;
    public float maxRotate = 45f;
    
    private Rigidbody rigitBody;
    private Animator animator;
    private GameManager manager = null;

    // Start is called before the first frame update
    void Awake()
    {
        rigitBody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        rigitBody.velocity = new Vector3(Speed, JumpBoost, 0);
        Physics.gravity = new Vector3(0, -Gravity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager)
            return;

        if (!manager.Dead)
        {
            if (manager.Running)
            {
                if (rigitBody.velocity.x != Speed || transform.position.y < -6.5f)
                {
                    manager.GameOver();
                }
                else
                {
#if UNITY_ANDROID
                if (Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Began)
#elif UNITY_STANDALONE || UNITY_WEBPLAYER
                    if (Input.GetButtonDown("Jump"))
#endif
                        jump();
                }
            } else if (transform.position.y < manager.MenuJumpThreshold)
            {
                jump();
            }

            transform.rotation = new Quaternion(0, 0, Mathf.Clamp(rigitBody.velocity.y / rotateFactor, -1, 1) * maxRotate, 1);
        }

    }

    private void jump()
    {
        animator.Play(FlyAnimationName);
        rigitBody.velocity = new Vector3(Speed, JumpBoost, 0);
    }

    public void RegisterManager(GameManager manager)
    {
        this.manager = manager;
    }
}
