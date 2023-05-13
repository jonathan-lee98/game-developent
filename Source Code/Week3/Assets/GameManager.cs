using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public float Health;
    public int coins;
    public int hearts;
    public float Timer;
    public Image TimerImage;
    public Text CoinsText;
    public Text HeartText;
    public bool timerpause;

    public bool redwallmove;
    public static GameManager instance = null;
    void Start()
    {
        hearts = 3;
        redwallmove = false;
        instance = this;
        timerpause = false;
        Timer = 1;
        Health = 1;
    }
    public void TurnOnRedWallMove()
    {
        redwallmove = true;
        GameObject.FindObjectOfType<PlayerController>().changeStatus();
   
    }
    // Update is called once per frame
    void Update()
    {
        if(timerpause==false)
        {
            TimerImage.fillAmount = Timer;
            Timer = Timer - (0.01f * Time.deltaTime);
        }

        CoinsText.text = coins.ToString();
        HeartText.text = hearts.ToString();
    }
}
