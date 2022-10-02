using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform tran_Cam;
    public float moveRate;
    public bool lockY = false;
    
    private float startPoint_x;
    private float startPoint_y;

    // Start is called before the first frame update
    void Start()
    {
        startPoint_x = transform.position.x;
        startPoint_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (lockY)
        {
            transform.position = new Vector2(startPoint_x + moveRate * tran_Cam.position.x, transform.position.y);
        } 
        else
        {
            transform.position = new Vector2(startPoint_x, startPoint_y + transform.position.y * moveRate);
        }
    }
}
