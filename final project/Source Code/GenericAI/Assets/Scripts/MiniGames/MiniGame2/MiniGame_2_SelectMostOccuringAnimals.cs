using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class MiniGame_2_SelectMostOccuringAnimals : MonoBehaviour
{
    public Sprite[] AnimalsSprite;

    public int MostOccuringIndex;


    public List<Button> Btns;
    public int CurrentNumber;
    public float Timer = 5;
    public bool state = false;
    public Text TimerText;

    public GameObject[] Rows;
    public List<int> AnimalsCount;
    public List<int> MaxAnimalsCount;
    int CorrectButtonsPressed;
    
    public void InitAnimals()
    {
        Btns.Clear();
        AnimalsCount.Clear();
        MostOccuringIndex = 0;
        CorrectButtonsPressed = 0;

        for (int x = 0; x < AnimalsSprite.Length; x++)
        {
            AnimalsCount.Add(0);
        }

        int indexcounter = 0;
        for (int i = 0; i < Rows.Length; i++)
        {
            for (int z = 0; z < Rows[i].transform.childCount; z++)
            {
                Btns.Add(Rows[i].transform.GetChild(z).gameObject.GetComponent<Button>());
                
                Btns[Btns.Count-1].interactable = true;

                indexcounter= (i * Rows[i].transform.childCount)+z;
              
                 // Btns[Btns.Count - 1].onClick.AddListener(() => ButtonPressed(indexcounter));

                 indexcounter++;
            }
        }

        for (int i = 0; i < Btns.Count; i++)
        {
          
                int r = Random.RandomRange(0, AnimalsSprite.Length);
                Btns[i].GetComponent<MiniGame_2_Button>().SpriteNumber = r;
                Btns[i].transform.GetComponent<Image>().color = Color.white;
               Btns[i].transform.GetChild(0).gameObject.GetComponent<Image>().sprite = AnimalsSprite[r];
                AnimalsCount[r]++;
                if (AnimalsCount[r] > AnimalsCount[MostOccuringIndex])
                    MostOccuringIndex = r;
            

        }     
        TimerText.color = Color.white;
        CurrentNumber = 0;
        Timer = 60;
        state = true;
        FillUpMaximumList();
    }
    public void FillUpMaximumList()
    {
        MaxAnimalsCount.Clear();
        for (int i=0;i<AnimalsCount.Count;i++)
        {
            if (AnimalsCount[i] == AnimalsCount[MostOccuringIndex])
                MaxAnimalsCount.Add(i);
        }
    }
    public void TurnAllButtonsOff()
    {
        for (int x = 0; x < Btns.Count; x++)
        {
            Btns[x].GetComponent<Button>().interactable = false;
        }
    }
    private void RemoveAllPossibleSequence(int i)
    {
        for(int x=0;x<MaxAnimalsCount.Count;x++)
        {
            if(MaxAnimalsCount[x]!=i)
            {
                MaxAnimalsCount.RemoveAt(x);
                x = 0;
            }
               
        }
    }
     public void ButtonPressed(int ButtonIndex)
    {


        if (MaxAnimalsCount.Contains(Btns[ButtonIndex].GetComponent<MiniGame_2_Button>().SpriteNumber))
        {
            RemoveAllPossibleSequence(Btns[ButtonIndex].GetComponent<MiniGame_2_Button>().SpriteNumber);

            Btns[ButtonIndex].GetComponent<Button>().interactable = false;
            Btns[ButtonIndex].transform.GetComponent<Image>().color = Color.green;
            //Btns.RemoveAt(ButtonIndex);

            CorrectButtonsPressed++;

            if (AnimalsCount[MostOccuringIndex] == CorrectButtonsPressed)
            {
                CloseMiniGame(true);
            }
        }
        else
        {
            Btns[ButtonIndex].transform.GetComponent<Image>().color = Color.red;
            TurnAllButtonsOff();
            state = false;
            Invoke("InitAnimals", 1f);
        }
    }
    void Start()
    {
        TimerText.text = "";
        Invoke("InitAnimals", 1f);
        //InitAnimals();
    }

    // Update is called once per frame
    void Update()
    {

        if (state)
        {
            Timer = Timer - Time.deltaTime;
            TimerText.text = ((int)Timer).ToString();
            if ((int)Timer == 10)
            {
                TimerText.color = Color.red;
            }
            if (Timer <= 0)
            {
                state = false;
                TurnAllButtonsOff();
                Invoke("InitAnimals", 1f);
            }
        }

    }

    public void CloseMiniGame(bool status)
    {
        if (status != true)
            this.gameObject.SetActive(false);

        else
        {
            MiniGameController.instance.StopMiniGame();
           

        }
    }

    public void QuitMiniGame()
    {
        MiniGameController.instance.StopMiniGame(false);
    }
}
