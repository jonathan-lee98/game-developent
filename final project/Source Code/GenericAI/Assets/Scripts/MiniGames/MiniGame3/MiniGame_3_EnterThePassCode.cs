using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniGame_3_EnterThePassCode : MonoBehaviour
{
    public GameObject[] Buttons;
    public List<int> Numbers;

    public bool state = false;

    public int CurrentPasswordCounter = 0;

    public Text[] EnteredPasswordsText;
    public int[] EnteredPasswords;
    public int[] CorrectPassword;


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
            Buttons[x].GetComponent<Button>().interactable = true;

        }

        for(int i=0;i<EnteredPasswordsText.Length;i++)
        {
            EnteredPasswordsText[i].text = "";
        }

        CurrentPasswordCounter = 0;


        //      TimerText.color = Color.white;

   //     Timer = 30;
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


        if(CorrectPassword[CurrentPasswordCounter]==ButtonIndex)
        {
            EnteredPasswordsText[CurrentPasswordCounter].text = ButtonIndex.ToString();
            EnteredPasswordsText[CurrentPasswordCounter].color = Color.green;

            CurrentPasswordCounter++;
            if(CurrentPasswordCounter == 4)
            {
                CloseMiniGame(true);
            }
        }
        else
        {
            EnteredPasswordsText[CurrentPasswordCounter].color = Color.red;
            TurnAllButtonsOff();
            state = false;
            Invoke("InitDigits",1f);
        }
    }
    void Start()
    {
       // TimerText.text = "";
        InitDigits();
    }

    // Update is called once per frame
    void Update()
    {

        if(state)
        {
            //Timer = Timer - Time.deltaTime;
            //TimerText.text = ((int)Timer).ToString();
            //if((int)Timer==10)
            //{
            //    TimerText.color = Color.red;
            //}
            //if (Timer <= 0)
            //{
            //    state = false;
            //    TurnAllButtonsOff();
            //    Invoke("InitDigits", 1f);
            //}
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


    public void QuitMiniGame()
    {
        MiniGameController.instance.StopMiniGame(false);
    }
}
