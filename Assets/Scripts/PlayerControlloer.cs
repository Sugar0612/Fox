using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControlloer : MonoBehaviour
{

    [SerializeField]private Rigidbody2D rb;
    private Animator anim;
    private bool isGround = false;
    private int JumpCount = 2;
    private float gravityScale_buf;

    public Collider2D coll;
    public Collider2D Head_coll;

    public LayerMask ground;
    public float speed;
    public float jumpForce;

    private int cherry_count = 0;
    public Text cherry_text;
    public GameObject header, foot;

    public Joystick joystick;

    private bool isClimb = false;

    //private bool is_hurt = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        gravityScale_buf = rb.gravityScale;

#if UNITY_ANDROID
        joystick.gameObject.SetActive(true); 
#elif UNITY_STANDALONE_WIN
        joystick.gameObject.SetActive(false); 
#endif

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        SwitchAnim();

        CheckClimb();
        Climb();

        isGround = Physics2D.OverlapCircle(foot.transform.position, 0.2f, ground);
    }

    void Movement()
    {
        if (anim.GetBool("hurt")) return;

        float horizontal_dir = 0.0f;
        horizontal_dir =
#if UNITY_ANDROID
            joystick.Horizontal; // value [1, -1]
#elif UNITY_STANDALONE_WIN
            Input.GetAxis("Horizontal"); // value [1, -1]
#endif

        float facedir = 0.0f;
        facedir =
#if UNITY_ANDROID
            joystick.Horizontal; // value [1, -1]
#elif UNITY_STANDALONE_WIN
        Input.GetAxisRaw("Horizontal"); // value {1, 0, -1}
#endif

        //移动机制
        if (horizontal_dir != 0.0f)
        {
            /* Time.deltaTime 使在不同设备更加的平滑. */
            rb.velocity = new Vector2(horizontal_dir * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(facedir));
        }
        
#if UNITY_ANDROID
        if (facedir > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        } else {
            transform.localScale = new Vector3(-1, 1, 1);
        }
#elif UNITY_STANDALONE_WIN
        if (facedir != 0)
        {
            transform.localScale = new Vector3(facedir, 1, 1);
        }
#endif

        // 跳跃机制
        if (isGround)
        {
            JumpCount = 2;
        }

        if (
#if UNITY_ANDROID
            joystick.Vertical > 0.5f
#elif UNITY_STANDALONE_WIN
            Input.GetButtonDown("Jump")
#endif
            && JumpCount > 0)
        {
            SoundManager.Get().Jump();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            anim.SetBool("is_jumping", true);
            anim.SetBool("is_falling", false);
            JumpCount--;
        }

#region Crouch
        else if (!Physics2D.OverlapCircle(header.transform.position, 0.4f, ground))
        {
            if (
#if UNITY_ANDROID
                joystick.Vertical < -0.5f
#elif UNITY_STANDALONE_WIN
                Input.GetButtonDown("Crouch")
#endif
                )
            {
                anim.SetBool("is_crouch", true);
                Head_coll.enabled = false;
            }
            else if (Input.GetButtonUp("Crouch") || !Input.GetButton("Crouch"))
            {
                anim.SetBool("is_crouch", false);
                Head_coll.enabled = true;
            }
        }
#endregion Crouch
    }

    private void CheckClimb()
    {
        isClimb = Head_coll.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }

    private void Climb()
    {
        if(isClimb)
        {
            anim.SetBool("is_falling", false);

            rb.gravityScale = 0.0f;
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            anim.SetBool("is_climb", true);

            float moveY = Input.GetAxis("Vertical");
            if ((moveY > 0.5f || moveY < -0.5f)) {
                rb.velocity = new Vector2(0.0f, moveY * 4.0f);
                anim.SetBool("is_climbing", true);
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                anim.SetBool("is_climbing", false);
            }
        }
        else
        {
            anim.SetBool("is_climbing", false);
            anim.SetBool("is_climb", false);
            anim.StopPlayback();
            rb.gravityScale = 1.0f;
        }
    }

    void SwitchAnim() {
        anim.SetBool("is_idle", false);
       
        if(rb.velocity.y < 0.1 && !coll.IsTouchingLayers(ground) && !coll.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            Debug.Log("fall");
            anim.SetBool("is_falling", true);
        }

        if(anim.GetBool("is_jumping"))
        {
            if(rb.velocity.y < 0)
            {
                anim.SetBool("is_jumping", false);
                anim.SetBool("is_falling", true);
            }
        }
        else if(anim.GetBool("hurt"))
        {
            if(Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                anim.SetBool("is_idle", true);
                anim.SetFloat("speed", 0.0f);
            }
        }
        else if (coll.IsTouchingLayers(ground))
        {
            anim.SetBool("is_falling", false);
            anim.SetBool("is_idle", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "collectible")
        {
            Collectibles cherry = collision.gameObject.GetComponent<Collectibles>();
            cherry.Pick();
            SoundManager.Get().Pick();
        }
        else if (collision.tag == "DiedLine") {
            SoundManager.Get().Stop();

            //Delay..
            Invoke("Restart", 1.0f);
        }
    }

    private void Restart()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /* 消灭敌人.. */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy") {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("is_falling"))
            {           
                //Destroy(collision.gameObject);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
                anim.SetBool("is_jumping", true);
                SoundManager.Get().EnemDeath();
                enemy.Death();
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                SoundManager.Get().Hurt();
                anim.SetBool("is_cilmb", false);
                anim.SetBool("is_cilmbing", false);
                anim.SetBool("is_falling", false);
                anim.SetBool("hurt", true);
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                SoundManager.Get().Hurt();
                anim.SetBool("is_cilmb", false);
                anim.SetBool("is_cilmbing", false);
                anim.SetBool("is_falling", false);
                anim.SetBool("hurt", true);
                rb.velocity = new Vector2(5, rb.velocity.y);
            }
        } 
    }

    public void UpdateCherry()
    {
        cherry_count++;
        cherry_text.text = cherry_count.ToString();
    }
}
