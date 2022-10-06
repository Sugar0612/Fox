using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 我们需要把Frog和Eagle中相同的功能都放在Enemy中.
 */

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Animator anim;
    protected AudioSource deathAudio;
    protected void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>(); //音乐集成.
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death() // Frog和Eagle调用死亡动画..
    {
        //deathAudio.Play();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("is_death");
    }

    public void DestroyEnemy() //Frog和Eagle死亡动画结束后事件调用..
    {
        Destroy(gameObject);
    }
}
