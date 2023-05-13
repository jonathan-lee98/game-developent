using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniGame_1_EnterNumberInSortedOrder : MonoBehaviour
{
    public GameObject[] Buttons;
    public List<int> Numbers;
    public int CurrentNumber;
    public float Timer = 5;
    public bool state = false;
    public Text TimerText;
    public void InitDigits()
    {
        Numbers = new List<int>();
        for (int i=0;i<Buttons.Length;i++)
        {
            Numbers.Add(i);
        }
        for(int x=0;x<Buttons.Length;x++)
        {
            Buttons[x].GetComponent<Image>().color = Color.white;

            int ind = UnityEngine.Random.RandomRange(0, Numbers.Count);
            Buttons[x].GetComponent<Button>().interactable = true;
            Buttons[x].transform.GetChild(0).GetComponent<Text>().text = Numbers[ind].ToString();
            Numbers.RemoveAt(ind);
        }
        TimerText.color = Color.white;
        CurrentNumber = 0;
        Timer = 30;
        state = true;
    }
    public void TurnAllButtonsOff()
    {
        for (int x = 0; x < Buttons.Length; x++)
        {
            Buttons[x].GetComponent<Button>().interactable = false;
        }
    }
        public void ButtonPressed( int ButtonIndex)
    {
        if(Buttons[ButtonIndex].transform.GetChild(0).GetComponent<Text>().text==CurrentNumber.ToString())
        {
            //Buttons[ButtonIndex].GetComponent<Button>().interactable = false;
            Buttons[ButtonIndex].transform.GetComponent<Image>().color = Color.green;
            CurrentNumber++;
            if(CurrentNumber==10)
            {
                CloseMiniGame(true);
            }
        }
        else
        {
            Buttons[ButtonIndex].transform.GetComponent<Image>().color = Color.red;
            TurnAllButtonsOff();
            state = false;
            Invoke("InitDigits",1f);
        }
    }
    void Start()
    {
        TimerText.text = "";
        Invoke("InitDigits", 1f);
        //InitDigits();
    }

    // Update is called once per frame
    void Update()
    {

        if(state)
        {
            Timer = Timer - Time.deltaTime;
            TimerText.text = ((int)Timer).ToString();
            if((int)Timer==10)
            {
                TimerText.color = Color.red;
            }
            if (Timer <= 0)
            {
                state = false;
                TurnAllButtonsOff();
                Invoke("InitDigits", 1f);
            }
        }
        
    }

    public void CloseMiniGame(bool status)
    {
        if (status != true)
            this.gameObject.SetActive(false);

        else
        {
           // Manager.instance.Player.GetComponent<Character>().TaskCompleted();
            //this.gameObject.SetActive(false);

            MiniGameController.instance.StopMiniGame();
        }
    }
}
