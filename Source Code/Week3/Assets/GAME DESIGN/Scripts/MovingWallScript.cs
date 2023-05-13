using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingWallScript : MonoBehaviour
{
    public float speed = 1.0f;
    public Transform MoveToPosition;
    public bool islock;

    void Start()
    {
        islock = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(islock)
        {
            float step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, MoveToPosition.localPosition, step);

            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, MoveToPosition.localPosition) < 0.001f)
            {
             islock=false;
            }
        }
    }
}
