using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    protected Animator anim;

    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        anim.SetTrigger("is_death");
    }

    public void DestroyEnemy()
    {
        Destroy(gameObject);
    }
}
