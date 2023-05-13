using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public bool toogleredwallmove;
    public Image redwallpowerimage;
    Vector3 initpositon;
    public float movementspeed=10;
    public float jumpspeed = 16;

    private float initmovespeed;
    private float initjspeed;
    public Rigidbody2D rb;
    private Animator anim;

    float counter;
    void Start()
    {
        counter = 0;
        initpositon = this.transform.localPosition;
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
        initmovespeed = movementspeed;
        initjspeed = jumpspeed;
        toogleredwallmove = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="RedWall")
        {
            Debug.Log("APPLYING FORCE");
            if (toogleredwallmove == false)
            {
                Debug.Log("APPLYING FORCE");
                rb.AddForce(new Vector2(0, -jumpspeed), ForceMode2D.Impulse);
            }
            else
            movementspeed = 2;

        }
       


    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        if (collision.gameObject.tag=="Enemy")
        {
          
            this.transform.localPosition = initpositon;
        }
        if (collision.gameObject.tag == "Coin")
        {
            GameManager.instance.coins++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "Heart")
        {
            GameManager.instance.hearts++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.tag == "FinishPoint")
        {
            SceneManager.LoadScene("gAMEcOMPLETE");
        }

        if (collision.gameObject.tag == "RedWall")
        {
            
                rb.AddForce(new Vector2(0, -13), ForceMode2D.Impulse);
    
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "RedWall")
        {
            movementspeed = initmovespeed;

        }
    }
    private void OnCollisionEnter2D(Collision collision)
    {
        Debug.Log("HAROON");
     //   rb.AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
    }
    public void changeStatus()
    {
        if (toogleredwallmove == true)
        {
            //toogleredwallmove = false;
            //var color = redwallpowerimage.color;
            //color.a = 0.1f;
            //redwallpowerimage.color = color;

            //GameObject[] RedWAlls = GameObject.FindGameObjectsWithTag("RedWall");
            //for (int i = 0; i < RedWAlls.Length; i++)
            //{
            //    RedWAlls[i].GetComponent<BoxCollider2D>().isTrigger = false;
            //}
        }
        else
        {
            toogleredwallmove = true;
            counter = 0;
            var color = redwallpowerimage.color;
            color.a = 1f;
            redwallpowerimage.color = color;

            GameObject[] RedWAlls = GameObject.FindGameObjectsWithTag("RedWall");
            for (int i = 0; i < RedWAlls.Length; i++)
            {
                RedWAlls[i].GetComponent<BoxCollider2D>().isTrigger = true;
            }
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        LayerMask mask2 = LayerMask.GetMask("GROUNDLAYER");
        RaycastHit2D hit3 = Physics2D.CircleCast(this.gameObject.transform.position, 0.3f, new Vector2(this.transform.localScale.x, 0),1.3f,mask2);
        if (hit3.collider != null)
        {
            Debug.Log("NAME :" + hit3.collider.gameObject.name);
            rb.AddForce(new Vector2(0, -3), ForceMode2D.Impulse);

        }





        if (toogleredwallmove)
        {
         
            counter = counter + Time.deltaTime ;
            if(counter>=7)
                changeStatus();
        }
        if(Input.GetKeyDown(KeyCode.Q))
                   if (GameManager.instance.redwallmove)
                                 changeStatus();                  

        float x = Input.GetAxis("Horizontal");
        x = x * movementspeed;
        if(x<0)
            this.transform.localScale = new Vector3(-1, 1, 1);
        else
            this.transform.localScale = new Vector3(1, 1, 1);


        LayerMask mask = LayerMask.GetMask("GROUNDLAYER");

        if (Input.GetButtonDown("Jump"))
        {
            RaycastHit2D hit = Physics2D.CircleCast(this.gameObject.transform.position,0.4f, new Vector2(0, -1), 1.1f,mask);
            
            if(hit.collider == null)
            {
                rb.AddForce(new Vector2(0, -13), ForceMode2D.Impulse);
            }
            else if(hit.collider.gameObject!=null)
            {
                anim.SetBool("Jump", true);
                rb.AddForce(new Vector2(0, jumpspeed), ForceMode2D.Impulse);
            }
        }



        rb.velocity = new Vector2(x, rb.velocity.y);
        anim.SetInteger("MovementSpeed", (int)x);


    }
}
