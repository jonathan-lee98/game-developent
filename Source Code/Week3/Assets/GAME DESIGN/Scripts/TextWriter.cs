using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextWriter : MonoBehaviour
{
    public GameObject[] TurnOnThings;
    public float letterPause = 0.1f;
    public float fadeOutTime = 0.1f;
    public Text textComp;
    public string message;
    

    // Use this for initialization
    void Start()
    {
        if (message == ".")
            message = textComp.text;

        textComp.text = "";
        StarWriting();

        
    }
    public void StarWriting()
    {
       
     
        StartCoroutine(TypeText());
    }
    private IEnumerator FadeOutRoutine()
    {
        Text text = textComp.GetComponent<Text>();
        Color originalColor = text.color;
        for (float t = 0.01f; t < fadeOutTime; t += Time.deltaTime)
        {
           
            yield return null;
        }
    }

    IEnumerator TypeText()
    {

        for(int i=0;i<message.Length;i++)
        {
            char x = message[i];
            textComp.text += x;
         
            if (i%3==0)
            {
               
                yield return new WaitForSeconds(0.1f);
            }
          
        }


        for (int i = 0; i < TurnOnThings.Length; i++)
            TurnOnThings[i].SetActive(true);

        //foreach (char letter in message.ToCharArray())
        //{
        //    textComp.text += letter;
        //    yield return new WaitForSeconds(0.01f);
        //}


    }
}
