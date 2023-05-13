using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setTrigger : MonoBehaviour
{
    public Animator EathAnimator;

    void Start()
    {
        
    }
    public void  EartgTrigger()
    {
        EathAnimator.GetComponent<Animator>().SetBool("Answered",true);
        this.GetComponent<Animator>().SetTrigger("Answered");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
