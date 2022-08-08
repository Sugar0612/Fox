using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    private Collider2D coll;
    public Transform leftPoint, rightPoint;
    public LayerMask ground;
    public float speed, jumpForce;
    private bool FaceLeft;
    private float left_x, right_x;

   // private Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();

        left_x = leftPoint.position.x;
        right_x = rightPoint.position.x;
        FaceLeft = (transform.localScale.x == 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
       // Movement();
        SwitchAnim();
    }


    private void Movement()
    {
        if(FaceLeft)
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("is_jumping", true);
                anim.SetBool("is_idle", false);
                rb.velocity = new Vector2(-speed, jumpForce);
            }
            if(transform.position.x <= left_x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                FaceLeft = false;
            }
        }
        else
        {
            if (coll.IsTouchingLayers(ground))
            {
                anim.SetBool("is_jumping", true);
                anim.SetBool("is_idle", false);
                rb.velocity = new Vector2(speed, jumpForce);
            }
            if (transform.position.x >= right_x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                FaceLeft = true;
            }
        }
    }

    private void SwitchAnim()
    {
        if (anim.GetBool("is_jumping"))
        {
            if (rb.velocity.y < 0.1f) {
                anim.SetBool("is_jumping", false);
                anim.SetBool("is_falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("is_falling"))
        {
            anim.SetBool("is_falling", false);
            anim.SetBool("is_idle", true);
        }
    }
}
