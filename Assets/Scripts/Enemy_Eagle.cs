using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Eagle : Enemy
{
    public Transform up, down, left, right;
    public float speed_y;
    public float speed_x;

    private Rigidbody2D rb;
    private Collider2D coll;
    private bool isUp = true;
    private bool isLeft = true;
    private float Up_y, Down_y, Left_x, Right_x;

    // Start is called before the first frame update
    protected void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        Up_y = up.position.y;
        Down_y = down.position.y;
        Right_x = right.position.x;
        Left_x = left.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (isUp)
        {
            rb.velocity = new Vector2(rb.velocity.x, speed_y);
            if (transform.position.y >= Up_y)
            {
                isUp = false;
            }
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, -speed_y);
            if (transform.position.y <= Down_y)
            {
                isUp = true;
            }
        }

        if (isLeft)
        {
            rb.velocity = new Vector2(-speed_x, rb.velocity.y);
            if(transform.position.x <= Left_x)
            {
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                isLeft = false;
            }
        }
        else {
            rb.velocity = new Vector2(speed_x, rb.velocity.y);
            if(transform.position.x >= Right_x)
            {
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                isLeft = true;
            }
        }
    }
}
