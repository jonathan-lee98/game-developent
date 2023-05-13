using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class BallMovement : MonoBehaviour
{
    public int score = 0;
    public Text ScoreText;

    public Text LevelWinScore;

    public float moveSpeed = 5f;
    public float jumpForce = 10f;

    private Rigidbody rb;
    private bool isGrounded;

    public Vector3 StartPosition;

    public List<GameObject> NewPlatforms = new List<GameObject>();

    public GameObject LevelCompletePanel;

    void Start()
    {
        StartPosition = this.transform.position;
        rb = GetComponent<Rigidbody>();
    }


public void RetryGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitTheGame()
    {
        Application.Quit();
    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical);

        rb.AddForce(movement * moveSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if(NewPlatforms.Contains(collision.gameObject)==false)
            {
                score++;
                ScoreText.text = "Score: " + score;
                NewPlatforms.Add(collision.gameObject);
             //   this.transform.SetParent(collision.gameObject.transform);
            }
            
            isGrounded = true;
        }
        
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            score = 0;
            ScoreText.text = "Score: " + score;
            this.transform.position = StartPosition;
        }
        if (collision.gameObject.CompareTag("Destination"))
        {
            Time.timeScale = 0f;
            LevelCompletePanel.SetActive(true);
             LevelWinScore.text = "Score: " + score;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.transform.parent = null;
        }
    }
}
