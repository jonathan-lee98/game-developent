using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public int score = 0;
        public static GameManager instance;
    public Text ScoreTxt;
    public Text HealthTxt;
    public int Health = 3;
    public AudioSource PointSound;
    public GameObject GameFailedPanel;
    public Text GameFailed_Score;
    public AudioSource BuzzerSound;


    public float PlayerFreezeCooldown = 5f;
    public Toggle MovementToogle;

    public Slider Slider1;
    public Slider Slider2;


    private void Awake()
    {
        Screen.SetResolution(650, 1920, true);
        //Screen.SetResolution(1080, 1920, true);
        if (instance==null)
        {
            instance = this;
        }
    }
    public void DecrementHealth()
    {
        BuzzerSound.Play();
     Health--;
        HealthTxt.text = Health.ToString();
        if(Health<=0)
        {
            GameOver();
        }
    }
    public void GameOver()
    {
  
        Time.timeScale = 0f;
        GameFailed_Score.text = "Score:" + score.ToString();
        GameFailedPanel.SetActive(true);

    }
    public void IncrementScore()
    {
        PointSound.Play();
        score++;
        ScoreTxt.text = "Score:"+score.ToString();
    }
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    void Start()
    {
        
    }

    public bool Freezemovement;
    // Update is called once per frame
    public float difference1 = 0.1f;
    public float difference2 = 0.1f;
    void Update()
    {
        if(MovementToogle.isOn==true)
        {
            Slider1.value += difference1;
            Slider2.value += difference2;

            if(Slider1.value>=1 || Slider1.value <0.2)
            {
                difference1 = -difference1;
                if (Slider1.value >= 1)
                    Slider1.value = 0.9f;
                else
                    Slider1.value = 0.2f;
            }
            if (Slider2.value >= 1 || Slider2.value < 0.2)
            {
                difference2 = -difference2;

                if(Slider2.value >= 1)
                Slider2.value = 0.9f;
                else
                    Slider2.value = 0.2f;
            }
        }
    }
    public void ExitTheGame()
    {
        Application.Quit();
    }
    public void LoadTheGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
