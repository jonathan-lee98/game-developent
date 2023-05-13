using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
public class SignPostPng : MonoBehaviour
{
    public string SignBoardText;
    public GameObject SignBoardCanvas;
    public GameObject TextMasPro;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            GameManager.instance.timerpause = true;
            SignBoardCanvas.SetActive(true);
        }
   
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            GameManager.instance.timerpause = false;
            SignBoardCanvas.SetActive(false);
        }

    }
    void Start()
    {
        SignBoardCanvas.SetActive(false);
        TextMasPro.GetComponent<TMPro.TextMeshProUGUI>().text = SignBoardText;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
