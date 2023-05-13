using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedWallMovementScript : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.TurnOnRedWallMove();
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.instance.TurnOnRedWallMove();
        Destroy(this.gameObject);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
