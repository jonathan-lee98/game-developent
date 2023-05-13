using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObjectDisabler : MonoBehaviour
{
    public GameObject[] Objects;
    void Start()
    {
        for (int i = 0; i < Objects.Length; i++)
            Objects[i].SetActive(false);
        Invoke("LOADLEVEL", 4f);
    }
    public void LOADLEVEL()
    {
        SceneManager.LoadScene("GAMEPLAY");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
