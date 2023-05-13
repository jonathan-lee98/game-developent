using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public Vector3 StartPosition;
    public float PlatformDifferenceiny;
    public GameObject PlatformPrefab;
    public Material Destination;
    public int totalplatforms = 8;
    void Start()
    {
        Vector3 Pos = StartPosition;
        for(int i=0;i<totalplatforms;i++)
        {
            GameObject t = Instantiate(PlatformPrefab);
            t.SetActive(true) ;
            t.transform.localScale = new Vector3(UnityEngine.Random.RandomRange(7, 15), t.transform.localScale.y, t.transform.localScale.z);

            if(i==totalplatforms-1)
            {
       
                t.tag = "Destination";
                t.GetComponent<MeshRenderer>().material = Destination;
                t.transform.localScale = new Vector3(20, t.transform.localScale.y, t.transform.localScale.z);
                t.GetComponent<MovingPlatform>().enabled = false;
            }
            else
            {
                t.GetComponent<MovingPlatform>().SpawnObstacle();
                PlatformPrefab.transform.position = Pos;
                Pos += new Vector3(0, PlatformDifferenceiny, 0);
            }
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
