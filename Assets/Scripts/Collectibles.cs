using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    protected Animator anim;

    // Start is called before the first frame update
    protected void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Pick()
    {
        anim.SetTrigger("is_pick");
    }

    protected void Destroy()
    {
        Destroy(gameObject);
    }
}
