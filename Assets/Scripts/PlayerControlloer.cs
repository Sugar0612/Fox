using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerControlloer : MonoBehaviour
{

    [SerializeField]private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D Head_coll;

    public LayerMask ground;
    public float speed;
    public float jumpForce;

    public int cherry_count = 0;
    public Text cherry_text;
    public GameObject header;

    public AudioSource jumpAudio;
    public AudioSource HurtAudio;
    public AudioSource MainAudio;
    public Joystick joystick;

    //private bool is_hurt = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Movement();
        SwitchAnim();
    }

    void Movement()
    {
        if (anim.GetBool("hurt")) return;


#if UNITY_ANDROID
        float horizontal_dir = 0.0f;
        horizontal_dir = joystick.Horizontal; // value [1, -1]

        float facedir = 0.0f;
        facedir = joystick.Horizontal; // value {1, 0, -1}

        if(facedir > 0) {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (joystick.Vertical > 0.5f && coll.IsTouchingLayers(ground))
        {
            Debug.Log("Jumping");
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            anim.SetBool("is_jumping", true);
        }
        else if (!Physics2D.OverlapCircle(header.transform.position, 0.4f, ground))
        {
            if (joystick.Vertical < -0.5f)
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
#elif UNITY_EDITOR_WIN
        float horizontal_dir = 0.0f;
        horizontal_dir = Input.GetAxis("Horizontal"); // value [1, -1]

        float facedir = 0.0f;
        facedir = Input.GetAxisRaw("Horizontal"); // value {1, 0, -1}

        //移动机制
        if(horizontal_dir != 0.0f)
        {
            /* Time.deltaTime 使在不同设备更加的平滑. */
            rb.velocity = new Vector2(horizontal_dir * speed * Time.fixedDeltaTime, rb.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(facedir));
        }
        
        if(facedir != 0)
        {
            transform.localScale = new Vector3(facedir, 1, 1);
        }

        // 跳跃机制
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Debug.Log("Jumping");
            jumpAudio.Play();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.fixedDeltaTime);
            anim.SetBool("is_jumping", true);
        }

#region Crouch
        else if (!Physics2D.OverlapCircle(header.transform.position, 0.4f, ground))
        {
            if (Input.GetButtonDown("Crouch"))
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

#endif
    }

    void SwitchAnim() {
        anim.SetBool("is_idle", false);
       
        if(rb.velocity.y < 0.1 && coll.IsTouchingLayers(ground))
        {
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
        }
        else if (collision.tag == "DiedLine") {
            MainAudio.Stop();

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
                enemy.Death();
            }
            else if (transform.position.x < collision.gameObject.transform.position.x)
            {
                HurtAudio.Play();
                anim.SetBool("hurt", true);
                rb.velocity = new Vector2(-5, rb.velocity.y);
            }
            else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                HurtAudio.Play();
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
