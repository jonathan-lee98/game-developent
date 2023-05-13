using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BallController : MonoBehaviour
{
    public ColourType type;

    void Start()
    {

    }
    public float speed = 5f; // adjust this value to change the speed of falling
    void Update()
    {
        transform.position += Vector3.down * speed * Time.deltaTime;
        if (transform.position.y < 0f - Screen.height)
        {
            Destroy(gameObject);
        }
    }

    public void Pop()
    {
        if (type == ColourType.Other)
            GameManager.instance.IncrementScore();
        else
            GameManager.instance.DecrementHealth();

        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
     
        if (other.gameObject.tag == "end")
        {
            GameManager.instance.DecrementHealth();
            Destroy(this.gameObject);
        }
            if (other.gameObject.tag=="p1")
        {
            if(type==ColourType.Blue)
            {
                GameManager.instance.IncrementScore();
            }
            else
            {
                GameManager.instance.DecrementHealth();
            }

            Destroy(this.gameObject);
        }
        if (other.gameObject.tag == "p2")
        {
            if (type == ColourType.Green)
            {
                GameManager.instance.IncrementScore();
            }
            else
            {
                GameManager.instance.DecrementHealth();
            }
            Destroy(this.gameObject);
        }

    }

}
