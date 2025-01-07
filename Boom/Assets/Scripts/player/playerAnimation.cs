using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private playerControl Control;
    void Start()
    {
        anim=GetComponent<Animator>();
        rb=GetComponent<Rigidbody2D>();
        Control=GetComponent<playerControl>();
    }
    void Update()
    {
        anim.SetFloat("SpeedX",math.abs(rb.velocity.x));
        anim.SetFloat("VelocityY",rb.velocity.y);
        anim.SetBool("jump",Control.isJump);
        anim.SetBool("ground",Control.isGround);
    }
}
