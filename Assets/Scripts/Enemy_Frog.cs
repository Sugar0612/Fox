using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
    里面的代码都是关于Enemy_Frog的Move和死亡的, 我写了我认为比较精细的注释, 相信大家都可以看懂.
 */
 
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
        FaceLeft = (transform.localScale.x == 1.0f); //左边跳 or 右边跳.
    }

    // Update is called once per frame
    void Update()
    {
        //Movement(); 放在了 idle动画结束之后在进行移动..(动画事件)
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
            if (rb.velocity.y < 0.1f) { //当沿着y轴的跳跃结束时, 就应该开始下降了.
                anim.SetBool("is_jumping", false);
                anim.SetBool("is_falling", true);
            }
        }
        if (coll.IsTouchingLayers(ground) && anim.GetBool("is_falling")) //当接触到地面了, 那么就不应该下降了, 应该切换回idle状态.
        {
            anim.SetBool("is_falling", false);
            anim.SetBool("is_idle", true);
        }
    }
}
