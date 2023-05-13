using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float waitTime = 1f;
    public float minX = -5f;
    public float maxX = 5f;


    public Vector3 endPos;
    private float timer = 0f;
    private bool movingToEnd = true;



    public GameObject obstaclePrefab;

    private Transform platformTransform;
    private float platformScale;


    void Start()
    {
       
        minX = Random.Range(minX, 0);
        maxX = Random.RandomRange(0, maxX);
        movingToEnd = true;
        endPos = new Vector3(maxX, transform.position.y, transform.position.z);
    }

    void Update()
    {
     

       this.transform.position= Vector3.MoveTowards(this.transform.position, endPos, moveSpeed * Time.deltaTime);
        float distance = Vector3.Distance(transform.position, endPos);
        if (distance <= 0.1f)
        {
            if (movingToEnd)
            {
                endPos = new Vector3(minX, transform.position.y, transform.position.z);
                movingToEnd = false;
            }
            else
            {
                endPos = new Vector3(maxX, transform.position.y, transform.position.z);
                movingToEnd = true;
            }
        }
        
    }
    public void SpawnObstacle()
    {
        platformTransform = transform;
        platformScale = platformTransform.localScale.x;
     
        float obstacleXPos = Random.Range(platformTransform.position.x - platformScale / 2, platformTransform.position.x + platformScale / 2);
        Vector3 obstaclePos = new Vector3(obstacleXPos,transform.position.y+0.2f, obstaclePrefab.transform.position.z);
        GameObject t=Instantiate(obstaclePrefab, this.transform);

        t.transform.position = obstaclePos;
    }

}
