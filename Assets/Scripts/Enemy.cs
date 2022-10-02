using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Animator anim;
    protected AudioSource deathAudio;
    protected void Start()
    {
        anim = GetComponent<Animator>();
        deathAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        deathAudio.Play();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("is_death");
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
