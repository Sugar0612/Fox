using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    protected Animator anim;
    protected AudioSource pickAudio;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = GetComponent<Animator>();
        pickAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pick()
    {
        pickAudio.Play();
        GetComponent<Collider2D>().enabled = false;
        anim.SetTrigger("is_pick");
    }

    protected void Destroy()
    {
        FindObjectOfType<PlayerControlloer>().UpdateCherry();
        Destroy(gameObject);
    }
}
