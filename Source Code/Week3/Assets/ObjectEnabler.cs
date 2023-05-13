using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectEnabler : MonoBehaviour
{
    public GameObject TextAnimation;

    public void TurnOnText()
    {
        TextAnimation.SetActive(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
