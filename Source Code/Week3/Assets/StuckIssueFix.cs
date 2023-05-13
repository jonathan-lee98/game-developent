using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuckIssueFix : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HAROON");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {


        if(collision.gameObject.tag== "Player")
        {
            Debug.Log(collision.gameObject.tag);
            collision.gameObject.GetComponent<PlayerController>().GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
